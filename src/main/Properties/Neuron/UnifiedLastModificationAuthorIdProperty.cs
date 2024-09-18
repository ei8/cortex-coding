using System;

namespace ei8.Cortex.Coding.Properties.Neuron
{
    public class UnifiedLastModificationAuthorIdProperty : INeuronProperty<Guid?>
    {
        public UnifiedLastModificationAuthorIdProperty(Guid? value, string name)
        {
            this.Value = value;
            this.Name = name;
        }
        public Guid? Value { get; }

        public string Name { get; }

        public object GetValue() => this.Value;
    }
}
