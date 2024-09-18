using System;

namespace ei8.Cortex.Coding.Properties
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class neurULNeuronPropertyAttribute : Attribute
    {
        public neurULNeuronPropertyAttribute() : this(null)
        {
        }

        public neurULNeuronPropertyAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        public string PropertyName { get; }
    }
}
