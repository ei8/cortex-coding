using System;

namespace ei8.Cortex.Coding.Model.Properties
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false)]
    public class neurULKeyAttribute : Attribute
    {
        public neurULKeyAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; private set; }
    }
}
