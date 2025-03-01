﻿using ei8.Cortex.Coding.Properties;
using ei8.Cortex.Coding.Properties.Neuron;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.Reflection
{
    public class neurULizerTypeInfo
    {
        private neurULizerTypeInfo()
        { 
        }

        public string ValueClassKey { get; private set; }

        public IEnumerable<INeuronProperty> NeuronProperties { get; private set; }

        public IEnumerable<PropertyData> GrannyProperties { get; private set; }

        public IEnumerable<string> Keys =>
            new string[] {
                this.ValueClassKey
            }.Concat(
                this.GrannyPropertiesKeys
            );

        public IEnumerable<string> GrannyPropertiesKeys => 
            this.GrannyProperties.Select(gp => gp.Key)
                .Concat(this.GrannyProperties.Select(gp => gp.ClassKey))
                .Distinct();

        public IDictionary<string, Neuron> ExternalReferences { get; private set; }

        public static neurULizerTypeInfo GetTypeInfo<T>(T instance = null)
            where T : class
        {
            var result = new neurULizerTypeInfo();

            result.ValueClassKey = ExternalReference.ToKeyString(typeof(T));

            var propertyData = typeof(T).GetProperties()
                .Select(pi => pi.ToPropertyData(instance))
                .Where(pd => pd != null);

            result.NeuronProperties = propertyData.Where(pd => pd.NeuronProperty != null).Select(pd => pd.NeuronProperty);
            result.GrannyProperties = propertyData.Where(pd => pd.NeuronProperty == null);
            
            return result;
        }
    }
}
