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
        public static async Task<Neuron> GetExternalReferenceAsync(this IEnsembleRepository ensembleRepository, string userId, string key) =>
             (await ensembleRepository.GetExternalReferencesAsync(userId, key)).Values.SingleOrDefault();

        public static async Task<Neuron> GetExternalReferenceAsync(
            this IEnsembleRepository ensembleRepository,
            string userId,
            object key
            ) =>
            (await ensembleRepository.GetExternalReferencesAsync(userId, key)).Values.SingleOrDefault();

        public static async Task<IDictionary<object, Neuron>> GetExternalReferencesAsync(
            this IEnsembleRepository ensembleRepository,
            string userId,
            params object[] keys
            )
        {
            var keyConverter = new Func<object, string>(o =>
            {
                var result = o as string;
                if (o is Type)
                    result = ExternalReference.ToKeyString((Type)o);
                else if (o is Enum)
                    result = ExternalReference.ToKeyString((Enum)o);

                return result;
            });
            var origDict = await ensembleRepository.GetExternalReferencesAsync(userId, keys.Select(t => keyConverter(t)).ToArray());
            return origDict.ToDictionary(kvpK => keys.Single(t => keyConverter(t) == kvpK.Key), kvpE => kvpE.Value);
        }

        public static async Task Uniquify(this IEnsembleRepository ensembleRepository, string userId,
            Ensemble result)
        {
            var currentNeuronIds = result.GetItems<Neuron>()
                .Where(
                    n =>
                        n.IsTransient &&
                        result.GetPostsynapticNeurons(n.Id).All(postn => !postn.IsTransient)
                )
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
                        result.TryGetById(currentNeuronId, out Neuron currentNeuron),
                        $"'currentNeuron' '{currentNeuronId}' must exist in ensemble."
                    );

                    Debug.WriteLine($"Tag: '{currentNeuron.Tag}'");

                    var postsynaptics = result.GetPostsynapticNeurons(currentNeuronId);
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
                            postsynaptics,
                            userId
                        );

                        if (identical.Item2 != null && currentNeuron.Tag == identical.Item2.Tag)
                        {
                            Debug.WriteLine($"> Persistent identical granny found - updating presynaptics and containing ensemble.");
                            EnsembleRepositoryExtensions.UpdateDendrites(
                                result,
                                currentNeuronId,
                                identical.Item2.Id
                            );
                            EnsembleRepositoryExtensions.RemoveTerminals(
                                result,
                                currentNeuronId
                            );
                            result.AddReplaceItems(identical.Item1);
                            result.Remove(currentNeuronId);
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
                    var presynaptics = result.GetPresynapticNeurons(nextPostsynapticId);
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
            IEnumerable<Neuron> postsynaptics,
            string userId
        )
        {
            IEnumerable<string> postsIds = postsynaptics.Select(n => n.Id.ToString());

            var similarGrannyFromDb = (await ensembleRepository.GetByQueryAsync(
                    userId,
                    new Library.Common.NeuronQuery()
                    {
                        Postsynaptic = postsIds,
                        DirectionValues = Library.Common.DirectionValues.Outbound,
                        Depth = 1
                    }
                ));
            var similarGrannyFromDbNeuron =
                similarGrannyFromDb.GetItems<Neuron>()
                .Where(n => !postsynaptics.Any(pn => pn.Id == n.Id));

            AssertionConcern.AssertStateTrue(
                similarGrannyFromDbNeuron.Count() < 2,
                    $"Redundant Neuron with postsynaptic Neurons '{string.Join(", ", postsIds)}' encountered."
                );
            if (similarGrannyFromDbNeuron.Any())
            {
                var resultTerminalCount = similarGrannyFromDb.GetTerminals(similarGrannyFromDbNeuron.Single().Id).Count();
                AssertionConcern.AssertStateTrue(
                    resultTerminalCount == postsynaptics.Count(),
                    $"A correct identical match should have '{postsynaptics.Count()} terminals. Result has {resultTerminalCount}'."
                );
            }

            return new Tuple<Ensemble, Neuron>(
                similarGrannyFromDb,
                similarGrannyFromDbNeuron.SingleOrDefault()
                );
        }
    }
}
