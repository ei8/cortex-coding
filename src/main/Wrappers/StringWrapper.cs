using ei8.Cortex.Coding.Properties;
using neurUL.Common.Domain.Model;
using System;

namespace ei8.Cortex.Coding.Wrappers
{
    [neurULKey("System.String")]
    public class StringWrapper : IInstanceValueWrapper<string>
    {
        public StringWrapper() : this(null)
        {            
        }

        public StringWrapper(string value) : this(Guid.NewGuid(), value)
        {            
        }

        public StringWrapper(Guid id, string value)
        {
            AssertionConcern.AssertArgumentValid(i => i != Guid.Empty, id, $"Id cannot be equal to '{Guid.Empty}'.", nameof(id));

            this.Id = id;
            this.Value = value;
        }

        // TODO: should allow for null values getting persisted into and retrieved from brain
        public string Value { get; set; }

        public Guid Id { get; set; }

        public string Tag { get => this.Value; set => this.Value = value; }
    }
}