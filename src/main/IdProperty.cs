using System;

namespace ei8.Cortex.Coding
{
    public class IdProperty : INeuronProperty<Guid>
    {
        public IdProperty(Guid value)
        {
            this.Value = value;
        }

        public Guid Value { get; private set; }

        public object GetValue() => this.Value;
    }
}