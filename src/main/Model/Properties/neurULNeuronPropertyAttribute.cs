using System;

namespace ei8.Cortex.Coding.Model.Properties
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class neurULNeuronPropertyAttribute : Attribute
    {
        public neurULNeuronPropertyAttribute() : this(null)
        {
        }

        public neurULNeuronPropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; }
    }
}
