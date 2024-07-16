using neurUL.Common.Domain.Model;
using System;
using System.Text.RegularExpressions;

namespace ei8.Cortex.Coding
{
    public class PropertyData
    {
        public PropertyData(string key, string classKey, string value, ValueMatchBy valueMatchBy)
        {
            // TODO: Unnecessary since another service can be responsible for pruning grannies containing null or empty values
            //AssertionConcern.AssertArgumentValid(
            //    pv => valueMatchBy == ValueMatchBy.Tag && !string.IsNullOrWhiteSpace(pv),
            //    value,
            //    $"Property '{key}' cannot be null or whitespace when matching by '{valueMatchBy.ToString()}'.",
            //    nameof(value)
            //    );

            //AssertionConcern.AssertArgumentValid(
            //    pv => valueMatchBy == ValueMatchBy.Id && Guid.TryParse(pv, out Guid result),
            //    value,
            //    $"Property '{key}' must be a valid Guid when matching by '{valueMatchBy.ToString()}'.",
            //    nameof(value)
            //    );

            this.Key = key;
            this.ClassKey = classKey;
            this.Value = value;
            this.ValueMatchBy = valueMatchBy;
        }

        public PropertyData(INeuronProperty neuronProperty)
        {
            this.NeuronProperty = neuronProperty;
        }

        public string Key { get; }
        public string ClassKey { get; }
        public string Value { get; }
        public ValueMatchBy ValueMatchBy { get; }
        public INeuronProperty NeuronProperty { get; }
    }
}
