using ei8.Cortex.Coding.Mirrors;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ei8.Cortex.Coding
{
    public static class IEnumerableExtensions
    {
        public static bool HasSameElementsAs<T>(
            this IEnumerable<T> first,
            IEnumerable<T> second
            )
        {
            var firstMap = first
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());
            var secondMap = second
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());
            return
                firstMap.Keys.All(x =>
                    secondMap.Keys.Contains(x) && firstMap[x] == secondMap[x]
                ) &&
                secondMap.Keys.All(x =>
                    firstMap.Keys.Contains(x) && secondMap[x] == firstMap[x]
                );
        }

        public static bool TryGetMirrorNeuron(this IEnumerable<MirrorConfig> mirrorConfigs, string key, Network network, [NotNullWhen(true)] out Neuron? result)
        {
            result = null;

            return mirrorConfigs.TryGetByKey(
                key,
                out MirrorConfig? config
            ) &&
            network.TryGetByMirrorUrl(
                config.Url,
                out result
            );
        }

        public static bool TryGetByMirrorUrl(this Network network, string mirrorUrl, [NotNullWhen(true)] out Neuron? result)
        {
            bool bResult = false;
            result = null;
            var neurons = from n in network.GetItems<Neuron>()
                          where n.MirrorUrl == mirrorUrl
                          select n;

            if (neurons.Any())
            {
                result = neurons.Single();
                bResult = true;
            }

            return bResult;
        }

        public static bool TryGetByKey(this IEnumerable<MirrorConfig> configs, string key, [NotNullWhen(true)] out MirrorConfig? result)
        {
            result = configs.SingleOrDefault(c => c.Keys.Any(ck => ck == key));
            return result != null;
        }
    }
}
