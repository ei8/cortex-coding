using ei8.Cortex.Coding.Model.Properties.Neuron;

namespace ei8.Cortex.Coding.Model.Properties
{
    public class PropertyData
    {
        public PropertyData(string key, string classKey, string value, ValueMatchBy valueMatchBy)
        {
            Key = key;
            ClassKey = classKey;
            Value = value;
            ValueMatchBy = valueMatchBy;
        }

        public PropertyData(INeuronProperty neuronProperty)
        {
            NeuronProperty = neuronProperty;
        }

        public string Key { get; }
        public string ClassKey { get; }
        public string Value { get; }
        public ValueMatchBy ValueMatchBy { get; }
        public INeuronProperty NeuronProperty { get; }

        public string PropertyName => Key.Split(':')[1];
    }
}
