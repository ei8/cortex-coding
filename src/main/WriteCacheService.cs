using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding
{
    /// <summary>
    /// Provides functionality for caching Neurons during Write
    /// for usage in neurULization.
    /// </summary>
    public class WriteCacheService : IWriteCacheService
    {
        private readonly INetworkDictionary<CacheKey> readWriteCache;

        /// <summary>
        /// Contructs a WriteCacheService.
        /// </summary>
        /// <param name="readWriteCache"></param>
        public WriteCacheService(INetworkDictionary<CacheKey> readWriteCache)
        {
            AssertionConcern.AssertArgumentNotNull(readWriteCache, nameof(readWriteCache));

            this.readWriteCache = readWriteCache;
        }

        /// <summary>
        /// Save instance while caching Neuron data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="neuronCreator"></param>
        /// <param name="saver"></param>
        /// <returns></returns>
        public async Task SaveAsync<T>(T instance, CancellationToken cancellationToken, Func<T, Neuron> neuronCreator, Func<T, CancellationToken, Task> saver)
        {
            AssertionConcern.AssertArgumentNotNull(instance, nameof(instance));
            AssertionConcern.AssertArgumentNotNull(neuronCreator, nameof(neuronCreator));
            AssertionConcern.AssertArgumentNotNull(saver, nameof(saver));

            this.readWriteCache[CacheKey.Write].AddReplace(
                neuronCreator.Invoke(instance)
            );

            await saver(instance, cancellationToken);
        }

        /// <summary>
        /// Saves instances while caching Neuron data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instances"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="neuronCreator"></param>
        /// <param name="saver"></param>
        /// <returns></returns>
        public async Task SaveAllAsync<T>(IEnumerable<T> instances, CancellationToken cancellationToken, Func<T, Neuron> neuronCreator, Func<IEnumerable<T>, CancellationToken, Task> saver)
        {
            AssertionConcern.AssertArgumentNotNull(instances, nameof(instances));
            AssertionConcern.AssertArgumentNotNull(neuronCreator, nameof(neuronCreator));
            AssertionConcern.AssertArgumentNotNull(saver, nameof(saver));

            instances.ToList().ForEach(i =>
                this.readWriteCache[CacheKey.Write].AddReplace(
                    neuronCreator.Invoke(i)
                )
            );

            await saver(instances, cancellationToken);
        }
    }
}
