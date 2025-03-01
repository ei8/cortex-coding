﻿using ei8.Cortex.Coding.Properties.Neuron;

namespace ei8.Cortex.Coding.Properties
{
    public class PropertyData
    {
        public PropertyData(string key, string classKey, string value, ValueMatchBy valueMatchBy)
        {
            this.Key = key;
            this.ClassKey = classKey;
            this.Value = value;
            this.ValueMatchBy = valueMatchBy;
        }

        public PropertyData(INeuronProperty neuronProperty)
        {
            this.NeuronProperty = neuronProperty;
        }

        public string Key { get; }
        public string ClassKey { get; }
        public string Value { get; }
        public ValueMatchBy ValueMatchBy { get; }
        public INeuronProperty NeuronProperty { get; }

        public string PropertyName => this.Key.Split(':')[1];
    }
}
