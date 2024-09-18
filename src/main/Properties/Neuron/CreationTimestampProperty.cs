using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.Properties.Neuron
{
    public class CreationTimestampProperty : INeuronProperty<DateTimeOffset?>
    {
        public CreationTimestampProperty(DateTimeOffset? value, string name)
        {
            this.Value = value;
            this.Name = name;
        }

        public DateTimeOffset? Value { get; }

        public string Name { get; }

        public object GetValue() => this.Value;
    }
}
