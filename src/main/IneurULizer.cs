using ei8.Cortex.Coding.Reflection;
using System.Collections.Generic;

namespace ei8.Cortex.Coding
{
    public interface IneurULizer
    {
        /// <summary>
        /// neurULizes specified value.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="idPropertyValueNeurons"></param>
        /// <param name="typeInfo"></param>
        /// <param name="mirrors"></param>
        /// <returns></returns>
        Network neurULize<TValue>(
            TValue value,
            IEnumerable<Coding.Neuron> idPropertyValueNeurons,
            neurULizerTypeInfo typeInfo,
            IDictionary<string, Coding.Neuron> mirrors
        )
            where TValue : class;

        /// <summary>
        /// DeneurULizes specified value.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="instanceNeurons"></param>
        /// <param name="typeInfo"></param>
        /// <param name="mirrors"></param>
        /// <returns></returns>
        IEnumerable<neurULizationResult<TValue>> DeneurULize<TValue>(
            Network value, 
            IEnumerable<Neuron> instanceNeurons,
            neurULizerTypeInfo typeInfo,
            IDictionary<string, Coding.Neuron> mirrors
        )
            where TValue : class, new();

        IneurULizerOptions Options { get; }
    }
}