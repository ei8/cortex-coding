using System;

namespace ei8.Cortex.Coding.Model.Wrappers
{
    public interface IInstanceValueWrapper
    {
        Guid Id { get; set; }

        string Tag { get; set; }
    }

    public interface IInstanceValueWrapper<T> : IInstanceValueWrapper
    {
        T Value { get; set; }
    }
}
