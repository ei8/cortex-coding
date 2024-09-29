﻿using CQRSlite.Caching;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding
{
    public static class EnsembleRepositoryExtensions
    {
        public static async Task<Neuron> GetExternalReferenceAsync(this IEnsembleRepository ensembleRepository, string userId, string cortexLibraryOutBaseUrl, string key) =>
             (await ensembleRepository.GetExternalReferencesAsync(userId, cortexLibraryOutBaseUrl, key)).Values.SingleOrDefault();

        public static async Task<Neuron> GetExternalReferenceAsync(
            this IEnsembleRepository ensembleRepository,
            string userId,
            string cortexLibraryOutBaseUrl,
            object key
            ) =>
            (await ensembleRepository.GetExternalReferencesAsync(userId, cortexLibraryOutBaseUrl, key)).Values.SingleOrDefault();

        public static async Task<IDictionary<object, Neuron>> GetExternalReferencesAsync(
            this IEnsembleRepository ensembleRepository,
            string userId,
            string cortexLibraryOutBaseUrl,
            params object[] keys
            )
        {
            var keyConverter = new Func<object, string>(o =>
            {
                var result = o as string;
                if (o is MemberInfo)
                    result = ExternalReference.ToKeyString((MemberInfo)o);
                else if (o is Enum)
                    result = ExternalReference.ToKeyString((Enum)o);

                return result;
            });
            var origDict = await ensembleRepository.GetExternalReferencesAsync(
                userId, 
                cortexLibraryOutBaseUrl, 
                keys.Select(t => keyConverter(t)).ToArray()
            );
            return origDict.ToDictionary(kvpK => keys.Single(t => keyConverter(t) == kvpK.Key), kvpE => kvpE.Value);
        }

        public static async Task UniquifyAsync(
            this IEnsembleRepository ensembleRepository, 
            string appUserId,
            Ensemble ensemble,
            string cortexLibraryOutBaseUrl, 
            int queryResultLimit,
            IDictionary<string, Ensemble> cache = null
        )
        {
            await EnsembleRepositoryExtensions.UniquifyNeuronsAsync(
                ensembleRepository, 
                appUserId, 
                ensemble, 
                cortexLibraryOutBaseUrl, 
                queryResultLimit, 
                cache
            );
            await EnsembleRepositoryExtensions.UniquifyTerminalsAsync(
                ensembleRepository,
                appUserId,
                ensemble,
                cortexLibraryOutBaseUrl
            );           
        }

        private static async Task UniquifyNeuronsAsync(
            IEnsembleRepository ensembleRepository, 
            string appUserId, 
            Ensemble ensemble, 
            string cortexLibraryOutBaseUrl, 
            int queryResultLimit, 
            IDictionary<string, Ensemble> cache
        )
        {
            var currentNeuronIds = ensemble.GetItems<Neuron>()
                .Where(n => EnsembleRepositoryExtensions.IsTransientNeuronWithPersistentPostsynaptics(ensemble, n))
                .Select(n => n.Id);
            var nextNeuronIds = new List<Guid>();
            var processedNeuronIds = new List<Guid>();
            var removedNeuronIds = new List<Guid>();

            while (currentNeuronIds.Any())
            {
                nextNeuronIds.Clear();
                foreach (var currentNeuronId in currentNeuronIds.ToArray())
                {
                    Debug.WriteLine($"Optimizing '{currentNeuronId}'...");
                    if (removedNeuronIds.Contains(currentNeuronId))
                    {
                        Debug.WriteLine($"> Neuron replaced - skipped.");
                        continue;
                    }

                    AssertionConcern.AssertStateTrue(
                        ensemble.TryGetById(currentNeuronId, out Neuron currentNeuron),
                        $"'currentNeuron' '{currentNeuronId}' must exist in ensemble."
                    );

                    Debug.WriteLine($"Tag: '{currentNeuron.Tag}'");

                    var postsynaptics = ensemble.GetPostsynapticNeurons(currentNeuronId);
                    if (EnsembleRepositoryExtensions.ContainsTransientUnprocessed(postsynaptics, processedNeuronIds))
                    {
                        Debug.WriteLine($"> Transient unprocessed postsynaptic found - processing deferred.");
                        nextNeuronIds.Add(currentNeuronId);
                        continue;
                    }
                    else if (processedNeuronIds.Contains(currentNeuronId))
                    {
                        Debug.WriteLine($"> Already processed - skipped.");
                        continue;
                    }

                    var nextPostsynapticId = Guid.Empty;

                    if (currentNeuron.IsTransient)
                    {
                        Debug.WriteLine($"> Neuron marked as transient. Retrieving persistent identical granny with postsynaptics " +
                            $"'{string.Join(", ", postsynaptics.Select(n => n.Id))}'.");
                        var identical = await EnsembleRepositoryExtensions.GetPersistentIdentical(
                            ensembleRepository,
                            postsynaptics.Select(n => n.Id),
                            currentNeuron.Tag,
                            appUserId,
                            cortexLibraryOutBaseUrl,
                            queryResultLimit,
                            cache
                        );

                        if (identical != null)
                        {
                            Debug.WriteLine($"> Persistent identical granny found - updating presynaptics and containing ensemble.");
                            EnsembleRepositoryExtensions.UpdateDendrites(
                                ensemble,
                                currentNeuronId,
                                identical.Item2.Id
                            );
                            EnsembleRepositoryExtensions.RemoveTerminals(
                                ensemble,
                                currentNeuronId
                            );
                            ensemble.AddReplaceItems(identical.Item1);
                            ensemble.Remove(currentNeuronId);
                            removedNeuronIds.Add(currentNeuronId);
                            Debug.WriteLine($"> Neuron replaced and removed.");
                            nextPostsynapticId = identical.Item2.Id;
                        }
                        else
                        {
                            Debug.WriteLine($"> Persistent identical granny was NOT found.");
                            nextPostsynapticId = currentNeuronId;
                        }
                    }
                    else
                    {
                        Debug.WriteLine($"> Neuron NOT marked as transient.");
                        nextPostsynapticId = currentNeuronId;
                    }

                    processedNeuronIds.Add(nextPostsynapticId);
                    var presynaptics = ensemble.GetPresynapticNeurons(nextPostsynapticId);
                    presynaptics.ToList().ForEach(n =>
                    {
                        Debug.WriteLine($"> Adding presynaptic '{n.Id}' to nextNeuronIds.");
                        nextNeuronIds.Add(n.Id);
                    });
                }
                Debug.WriteLine($"Setting next batch of {nextNeuronIds.Count()} ids.");
                currentNeuronIds = nextNeuronIds.ToArray();
            }
        }

        private static async Task UniquifyTerminalsAsync(
            IEnsembleRepository ensembleRepository,
            string appUserId,
            Ensemble ensemble,
            string cortexLibraryOutBaseUrl
        )
        {
            var terminalIds = ensemble.GetItems<Terminal>()
                .Where(t => IsTransientTerminalLinkingPersistentNeurons(ensemble, t))
                .Select(t => t.Id);

            foreach(var tId in terminalIds)
            {
                if(
                    ensemble.TryGetById(tId, out Terminal currentTerminal) &&
                    await EnsembleRepositoryExtensions.HasPersistentIdentical(
                        ensembleRepository,
                        currentTerminal.PresynapticNeuronId,
                        currentTerminal.PostsynapticNeuronId,
                        appUserId,
                        cortexLibraryOutBaseUrl
                    )
                )
                ensemble.Remove(tId);
            }
        }

        private static async Task<bool> HasPersistentIdentical(
            IEnsembleRepository ensembleRepository, 
            Guid presynapticNeuronId, 
            Guid postsynapticNeuronId, 
            string appUserId, 
            string cortexLibraryOutBaseUrl
        )
        {
            var queryResult = await ensembleRepository.GetByQueryAsync(
                    appUserId,
                    new Library.Common.NeuronQuery()
                    {
                        Id = new string[] { presynapticNeuronId.ToString() },
                        Postsynaptic = new string[] { postsynapticNeuronId.ToString() },
                        // TODO: how should this be handled
                        NeuronActiveValues = Library.Common.ActiveValues.All,
                        TerminalActiveValues = Library.Common.ActiveValues.All
                    },
                    cortexLibraryOutBaseUrl,
                    int.MaxValue
                );

            return queryResult.GetItems<Neuron>().Any();
        }

        private static bool IsTransientTerminalLinkingPersistentNeurons(Ensemble ensemble, Terminal t)
        {
            return t.IsTransient &&
                ensemble.TryGetById(t.PresynapticNeuronId, out Neuron pre) &&
                ensemble.TryGetById(t.PostsynapticNeuronId, out Neuron post) &&
                !pre.IsTransient &&
                !post.IsTransient;
        }

        private static bool IsTransientNeuronWithPersistentPostsynaptics(Ensemble ensemble, Neuron neuron) =>
            neuron.IsTransient && ensemble.GetPostsynapticNeurons(neuron.Id).All(postn => !postn.IsTransient);

        private static void UpdateDendrites(Ensemble result, Guid oldPostsynapticId, Guid newPostsynapticId)
        {
            var currentDendrites = result.GetDendrites(oldPostsynapticId).ToArray();
            foreach (var currentDendrite in currentDendrites)
            {
                result.AddReplace(
                    new Terminal(
                        currentDendrite.Id,
                        currentDendrite.IsTransient,
                        currentDendrite.PresynapticNeuronId,
                        newPostsynapticId,
                        currentDendrite.Effect,
                        currentDendrite.Strength
                    )
                );
            }
        }

        private static void RemoveTerminals(Ensemble result, Guid presynapticId)
        {
            var terminals = result.GetTerminals(presynapticId).ToArray();
            foreach (var terminal in terminals)
                result.Remove(terminal.Id);
        }

        private static bool ContainsTransientUnprocessed(
            IEnumerable<Neuron> posts,
            IEnumerable<Guid> processedNeuronIds
        ) => posts.Any(n => n.IsTransient && !processedNeuronIds.Contains(n.Id));

        private static async Task<Tuple<Ensemble, Neuron>> GetPersistentIdentical(
            IEnsembleRepository ensembleRepository,
            IEnumerable<Guid> currentPostsynapticIds,
            string currentTag,
            string appUserId,
            string cortexLibraryOutBaseUrl, 
            int queryResultLimit,
            IDictionary<string, Ensemble> cache = null
        )
        {
            Tuple<Ensemble, Neuron> result = null;

            var similarGrannyFromCacheOrDb = await EnsembleRepositoryExtensions.GetEnsembleFromCacheOrDB(
                ensembleRepository,
                appUserId,
                cache,
                currentTag,
                currentPostsynapticIds,
                cortexLibraryOutBaseUrl,
                queryResultLimit
            );

            if (similarGrannyFromCacheOrDb != null)
            {
                var similarGrannyFromDbNeuron =
                    similarGrannyFromCacheOrDb.GetItems<Neuron>()
                    .Where(n => !currentPostsynapticIds.Any(pn => pn == n.Id));

                AssertionConcern.AssertStateTrue(
                    similarGrannyFromDbNeuron.Count() < 2,
                        $"Redundant Neuron with postsynaptic Neurons '{string.Join(", ", currentPostsynapticIds)}' encountered."
                    );
                if (similarGrannyFromDbNeuron.Any())
                {
                    var resultTerminalCount = similarGrannyFromCacheOrDb.GetTerminals(similarGrannyFromDbNeuron.Single().Id).Count();
                    AssertionConcern.AssertStateTrue(
                        resultTerminalCount == currentPostsynapticIds.Count(),
                        $"A correct identical match should have '{currentPostsynapticIds.Count()} terminals. Result has {resultTerminalCount}'."
                    );
                }

                result = new Tuple<Ensemble, Neuron>(
                    similarGrannyFromCacheOrDb,
                    similarGrannyFromDbNeuron.SingleOrDefault()
                );
            }

            return result;
        }

        private static async Task<Ensemble> GetEnsembleFromCacheOrDB(
            IEnsembleRepository ensembleRepository, 
            string appUserId, 
            IDictionary<string, Ensemble> cache,
            string currentTag,
            IEnumerable<Guid> currentPostsynapticIds, 
            string cortexLibraryOutBaseUrl, 
            int queryResultLimit
        )
        {
            string cacheId = currentTag + string.Join(string.Empty, currentPostsynapticIds.OrderBy(g => g));
            if (cache == null || !cache.TryGetValue(cacheId, out Ensemble result))
            {
                var tempResult = await ensembleRepository.GetByQueryAsync(
                    appUserId,
                    new Library.Common.NeuronQuery()
                    {
                        Tag = !string.IsNullOrEmpty(currentTag) ? new string[] { currentTag } : null,
                        Postsynaptic = currentPostsynapticIds.Select(pi => pi.ToString()),
                        DirectionValues = Library.Common.DirectionValues.Outbound,
                        Depth = 1
                    },
                    cortexLibraryOutBaseUrl,
                    queryResultLimit
                );

                if (tempResult.GetItems().Count() > 0)
                {
                    result = tempResult;

                    if (cache != null)
                        cache.Add(cacheId, result);
                }
                else
                    result = null;
            }

            return result;
        }
    }
}
