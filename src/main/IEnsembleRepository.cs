using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding
{
    public interface IEnsembleRepository
    {
        Task<IDictionary<string, Neuron>> GetExternalReferencesAsync(IEnumerable<string> keys);

        Task<IDictionary<string, Neuron>> GetExternalReferencesAsync(IEnumerable<string> keys, string userId);

        Task<Ensemble> GetByQueryAsync(NeuronQuery query);

        Task<Ensemble> GetByQueryAsync(NeuronQuery query, bool restrictQueryResultCount);

        Task<Ensemble> GetByQueryAsync(NeuronQuery query, string userId);

        Task<Ensemble> GetByQueryAsync(NeuronQuery query, string userId, bool restrictQueryResultCount);
    }
}
