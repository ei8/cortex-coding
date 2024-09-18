using System;

namespace ei8.Cortex.Coding.Properties.Neuron
{
    public class UnifiedLastModificationTimestampProperty : INeuronProperty<DateTimeOffset?>
    {
        public UnifiedLastModificationTimestampProperty(DateTimeOffset? value, string name)
        {
            this.Value = value;
            this.Name = name;
        }

        public DateTimeOffset? Value { get; }

        public string Name { get; }

        public object GetValue() => this.Value;
    }
}
