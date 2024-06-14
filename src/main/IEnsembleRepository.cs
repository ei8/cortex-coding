using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding
{
    public interface IEnsembleRepository
    {
        Task<IDictionary<string, Neuron>> GetExternalReferencesAsync(string userId, params string[] keys);

        Task<Ensemble> GetByQueryAsync(string userId, NeuronQuery query);
    }
}
