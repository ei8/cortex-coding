using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding
{
    public static class IEnsembleRepositoryExtensions
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
    }
}
