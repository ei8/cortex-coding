﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding
{
    public static class LibraryExtensions
    {
        public static Ensemble ToEnsemble(this Library.Common.QueryResult<Library.Common.Neuron> queryResult)
        {
            var allNs = queryResult.Items;
            allNs = allNs.Concat(queryResult.Items.SelectMany(n => n.Traversals.SelectMany(t => t.Neurons)));
            var allTs = queryResult.Items.SelectMany(n => n.Traversals.SelectMany(t => t.Terminals));

            var eNs = allNs.GroupBy(n => n.Id)
                .Select(g => g.First())
                .Select(n => n.ToEnsemble());
            var eTs = allTs.GroupBy(t => t.Id)
                .Select(g => g.First())
                .Select(t => t.ToEnsemble());

            return new Ensemble(
                eNs.Cast<IEnsembleItem>().Concat(
                    eTs.Cast<IEnsembleItem>()
                ).ToDictionary(ei => ei.Id)
            );
        }

        public static Ensemble ToEnsemble(this IEnumerable<Library.Common.QueryResult<Library.Common.Neuron>> queryResults)
        {
            var allNs = queryResults.SelectMany(qr => qr.Items.SelectMany(n => n.Traversals.SelectMany(t => t.Neurons)));
            var allTs = queryResults.SelectMany(qr => qr.Items.SelectMany(n => n.Traversals.SelectMany(t => t.Terminals)));

            var eNs = allNs.GroupBy(n => n.Id)
                .Select(g => g.First())
                .Select(n => n.ToEnsemble());
            var eTs = allTs.GroupBy(t => t.Id)
                .Select(g => g.First())
                .Select(t => t.ToEnsemble());

            return new Ensemble(
                eNs.Cast<IEnsembleItem>().Concat(
                    eTs.Cast<IEnsembleItem>()
                ).ToDictionary(ei => ei.Id)
            );
        }

        public static Neuron ToEnsemble(
            this Library.Common.Neuron value
            ) => new Neuron(
                Guid.Parse(value.Id),
                value.Tag,
                value.ExternalReferenceUrl,
                Guid.TryParse(value.Region?.Id, out Guid regionId) ? regionId : (Guid?)null,
                value.Region?.Tag,
                DateTimeOffset.TryParse(value.Creation.Timestamp, out DateTimeOffset creationTimestamp) ? creationTimestamp : (DateTimeOffset?)null,
                Guid.TryParse(value.Creation.Author.Id, out Guid authorId) ? authorId : Guid.Empty,
                value.Creation.Author.Tag,
                DateTimeOffset.TryParse(value.UnifiedLastModification.Timestamp, out DateTimeOffset unifiedLastModificationTimestamp) ? unifiedLastModificationTimestamp : (DateTimeOffset?)null,
                Guid.TryParse(value.UnifiedLastModification.Author.Id, out Guid unifiedAuthorId) ? unifiedAuthorId : (Guid?)null,
                value.UnifiedLastModification.Author.Tag,
                value.Url,
                value.Version
            );

        public static Terminal ToEnsemble(
            this Library.Common.Terminal value
        )
        {
            var result = new Terminal(
                Guid.Parse(value.Id),
                Guid.Parse(value.PresynapticNeuronId),
                Guid.Parse(value.PostsynapticNeuronId),
                Enum.TryParse(value.Effect, out NeurotransmitterEffect ne) ? ne : NeurotransmitterEffect.Excite,
                float.Parse(value.Strength)
                );

            return result;
        }
    }
}
