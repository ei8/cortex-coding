using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding.Properties.Neuron
{
    public class CreationAuthorTagProperty : INeuronProperty<string>
    {
        public CreationAuthorTagProperty(string value, string name)
        {
            this.Value = value;
            this.Name = name;
        }

        public string Value { get; }

        public string Name { get; }

        public object GetValue() => this.Value;
    }
}
