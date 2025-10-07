using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding
{
    /// <summary>
    /// Provides functionality for caching Neurons during Write
    /// for usage in neurULization.
    /// </summary>
    public interface IWriteCacheService
    {
        /// <summary>
        /// Save instance while caching Neuron data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="neuronCreator"></param>
        /// <param name="saver"></param>
        /// <returns></returns>
        Task SaveAsync<T>(
            T instance, 
            CancellationToken cancellationToken, 
            Func<T, Neuron> neuronCreator, 
            Func<T, CancellationToken, Task> saver
        );

        /// <summary>
        /// Saves instances while caching Neuron data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instances"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="neuronCreator"></param>
        /// <param name="saver"></param>
        /// <returns></returns>
        Task SaveAllAsync<T>(
            IEnumerable<T> instances, 
            CancellationToken cancellationToken, 
            Func<T, Neuron> neuronCreator, 
            Func<IEnumerable<T>, 
            CancellationToken, Task> saver
        );
    }
}
