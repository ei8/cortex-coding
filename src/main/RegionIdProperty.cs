using System;

namespace ei8.Cortex.Coding
{
    public class RegionIdProperty : INeuronProperty<Guid?>
    {
        public RegionIdProperty(Guid? value)
        {
            this.Value = value;
        }

        public Guid? Value { get; private set; }

        public object GetValue() => this.Value;
    }
}
