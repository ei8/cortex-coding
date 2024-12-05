using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding
{
    public interface IExternalReferenceRepository
    {
        Task Save(IEnumerable<ExternalReference> values);

        Task<IEnumerable<ExternalReference>> GetAllMissingAsync(IEnumerable<string> keys);

        Task<IDictionary<string, Neuron>> GetByKeysAsync(IEnumerable<string> keys);
    }
}
