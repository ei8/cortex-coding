using System;

namespace ei8.Cortex.Coding
{
    public class neurULNeuronPropertyAttribute : Attribute
    {
        public neurULNeuronPropertyAttribute() : this(null)
        {            
        }

        public neurULNeuronPropertyAttribute(string propertyName) : this(propertyName, false)
        {
            
        }

        public neurULNeuronPropertyAttribute(bool isReadOnly) : this(null, isReadOnly)
        {
        }

        public neurULNeuronPropertyAttribute(string propertyName, bool isReadOnly)
        {
            this.PropertyName = propertyName;
            this.IsReadOnly = isReadOnly;
        }

        public string PropertyName { get; }

        public bool IsReadOnly { get; }
    }
}
