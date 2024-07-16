using System;

namespace ei8.Cortex.Coding
{
    public class neurULKeyAttribute : Attribute
    {
        public neurULKeyAttribute(string key)
        {
            this.Key = key;
        }

        public string Key { get; private set; }
    }
}
