using neurUL.Common.Domain.Model;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding
{
    public static class NetworkExtensions
    {
        public static void ValidateIds(this Network value, IEnumerable<Guid> ids)
        {
            var missingIds = ids.Where(id => !value.TryGetById(id, out Neuron result));

            AssertionConcern.AssertStateTrue(
                !missingIds.Any(),
                $"Failed getting Neurons with IDs: " +
                $"'{string.Join(", ", missingIds)}'."
            );
        }

        /// <summary>
        /// Gets a Neuron from the Network using the specified Id and throws an exception if it is not found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="id"></param>
        /// <param name="neuronProcessor"></param>
        /// <returns></returns>
        public static T GetValidateNeuron<T>(this Network value, Guid id, Func<Neuron, T> neuronProcessor) =>
            neuronProcessor(NetworkExtensions.GetValidateNeuronCore(value, id));

        /// <summary>
        /// Asynchronously gets a Neuron from the Network using the specified Id
        /// and throws an exception if it is not found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="id"></param>
        /// <param name="neuronProcessor"></param>
        /// <returns></returns>
        public static async Task<T> GetValidateNeuronAsync<T>(this Network value, Guid id, Func<Neuron, Task<T>> neuronProcessor) =>
            await neuronProcessor(NetworkExtensions.GetValidateNeuronCore(value, id));

        private static Neuron GetValidateNeuronCore(Network value, Guid id)
        {
            if (!value.TryGetById(id, out Neuron result))
                throw new InvalidOperationException($"Neuron with Id '{id}' not found in 'readWriteCache'.");

            return result;
        }
    }
}
