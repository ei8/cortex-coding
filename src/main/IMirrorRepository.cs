using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding
{
    public interface IMirrorRepository
    {
        Task<bool> Initialize(IEnumerable<string> keys);

        Task Save(IEnumerable<Neuron> values);

        Task<IEnumerable<MirrorConfig>> GetAllMissingAsync(IEnumerable<string> keys);

        Task<IDictionary<string, Neuron>> GetByKeysAsync(IEnumerable<string> keys, bool throwErrorIfMissing = true);
    }
}
