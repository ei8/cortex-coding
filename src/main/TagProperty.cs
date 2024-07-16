using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding
{
    public class TagProperty : INeuronProperty<string>
    {
        public TagProperty(string value)
        {
            this.Value = value;
        }

        public string Value { get; private set; }

        public object GetValue() => this.Value;
    }
}
