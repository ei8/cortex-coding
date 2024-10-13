using System;

namespace ei8.Cortex.Coding
{
    public interface IEnsembleItem
    {
        Guid Id { get; }

        bool IsTransient { get; }
    }
}
