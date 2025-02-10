using System;

namespace ei8.Cortex.Coding
{
    public interface INetworkItem
    {
        Guid Id { get; }

        bool IsTransient { get; }
    }
}
