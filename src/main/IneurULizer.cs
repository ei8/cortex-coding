using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding
{
    public interface IneurULizer
    {
        Task<Ensemble> neurULizeAsync<TValue>(TValue value, IneurULizationOptions options);
    }
}
