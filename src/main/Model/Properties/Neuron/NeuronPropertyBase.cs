using neurUL.Common.Domain.Model;
using System;

namespace ei8.Cortex.Coding.Model.Properties.Neuron
{
    public abstract class NeuronPropertyBase<TValue> : INeuronProperty
    {
        internal NeuronPropertyBase() { }

        public TValue Value { get; private set; }

        public string Name { get; private set; }

        public bool HasValue { get; private set; }

        public static TNeuronProperty Create<TNeuronProperty>(object value, string name)
            where TNeuronProperty : NeuronPropertyBase<TValue>, new()
        {
            var t = new TNeuronProperty();

            bool nullable = !typeof(TValue).IsValueType || 
                Nullable.GetUnderlyingType(typeof(TValue)) != null;
            if (t.HasValue = value != null)
                t.Value = (TValue)value;
            else if (t.HasValue = nullable)
                t.Value = default;

            t.Name = name;
            return t;
        }

    }
}
