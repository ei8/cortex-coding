﻿using System;

namespace ei8.Cortex.Coding.Properties
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class neurULClassAttribute : Attribute
    {
        public neurULClassAttribute() : this(null, null)
        {
        }

        public neurULClassAttribute(Type type) : this(type, null)
        {
        }

        public neurULClassAttribute(string url) : this(null, url)
        {
            throw new NotImplementedException();
        }

        private neurULClassAttribute(Type type, string url)
        {
            this.Type = type;
            this.Url = url;
        }

        public Type Type { get; }

        public string Url { get; }
    }
}
