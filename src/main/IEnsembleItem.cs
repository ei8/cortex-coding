using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding
{
    public interface IEnsembleItem
    {
        Guid Id { get; }

        bool IsTransient { get; }
    }
}
