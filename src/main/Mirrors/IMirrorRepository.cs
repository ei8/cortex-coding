using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.Mirrors
{
    /// <summary>
    /// Provides functionality for Mirror Repositories.
    /// </summary>
    public interface IMirrorRepository
    {
        /// <summary>
        /// Initializes Mirrors using the specified keys.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<bool> Initialize(IEnumerable<string> keys);

        /// <summary>
        /// Saves the specified Mirror neurons.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        Task Save(IEnumerable<Neuron> values);

        /// <summary>
        /// Gets the MirrorConfigs using the specified keys that are not found in Persistence.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<IEnumerable<MirrorConfig>> GetAllMissingAsync(IEnumerable<string> keys);

        /// <summary>
        /// Gets Mirror neurons by their Keys.
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="throwErrorIfMissing"></param>
        /// <returns></returns>
        Task<IDictionary<string, Neuron>> GetByKeysAsync(IEnumerable<string> keys, bool throwErrorIfMissing = true);
    }
}
