using ei8.Cortex.Coding.Model.Properties;
using ei8.Cortex.Coding.Model.Properties.Neuron;
using ei8.Cortex.Coding.Model.Wrappers;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.Model.Reflection
{
    public class neurULizerTypeInfo
    {
        private neurULizerTypeInfo()
        {
            ValueClassKey = string.Empty;
            NeuronProperties = Enumerable.Empty<INeuronProperty>();
            GrannyProperties = Enumerable.Empty<PropertyData>();
        }

        public string ValueClassKey { get; private set; }

        public IEnumerable<INeuronProperty> NeuronProperties { get; private set; }

        public IEnumerable<PropertyData> GrannyProperties { get; private set; }

        public IEnumerable<string> Keys =>
            new[] {
                ValueClassKey
            }.Concat(
                GrannyPropertiesKeys
            );

        public IEnumerable<string> GrannyPropertiesKeys =>
            GrannyProperties.Select(gp => gp.Key)
                .Concat(GrannyProperties.Select(gp => gp.ClassKey))
                .Distinct();

        public IDictionary<string, Neuron> Mirrors { get; private set; }

        public static neurULizerTypeInfo GetTypeInfo<T>(T instance = null)
            where T : class
        {
            var result = new neurULizerTypeInfo();

            result.ValueClassKey = typeof(T).ToKeyString();

            var propertyData = typeof(T).GetProperties()
                .Select(pi => pi.ToPropertyData(instance))
                .Where(pd => pd != null);

            result.NeuronProperties = propertyData.Where(pd => pd.NeuronProperty != null).Select(pd => pd.NeuronProperty);

            if (!typeof(IInstanceValueWrapper).IsAssignableFrom(typeof(T)))
                result.GrannyProperties = propertyData.Where(pd => pd.NeuronProperty == null);
            
            return result;
        }
    }
}
