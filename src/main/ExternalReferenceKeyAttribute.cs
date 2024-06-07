using System;

namespace ei8.Cortex.Coding
{
    public class ExternalReferenceKeyAttribute : Attribute
    {
        public ExternalReferenceKeyAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; private set; }
    }
}
