using ei8.Cortex.Coding.Reflection;
using System;
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
        /// <param name="typeInfo"></param>
        /// <param name="idPropertyValueNeurons"></param>
        /// <param name="externalReferences"></param>
        /// <returns></returns>
        Network neurULize<TValue>(
            TValue value, 
            neurULizerTypeInfo typeInfo,
            IDictionary<Guid, Coding.Neuron> idPropertyValueNeurons,
            IDictionary<string, Coding.Neuron> externalReferences            
        )
            where TValue : class;

        /// <summary>
        /// DeneurULizes specified value.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="instanceNeurons"></param>
        /// <param name="typeInfo"></param>
        /// <param name="externalReferences"></param>
        /// <returns></returns>
        IEnumerable<TValue> DeneurULize<TValue>(
            Network value, 
            IEnumerable<Neuron> instanceNeurons,
            neurULizerTypeInfo typeInfo,
            IDictionary<string, Coding.Neuron> externalReferences
        )
            where TValue : class, new();

        IneurULizerOptions Options { get; }
    }
}