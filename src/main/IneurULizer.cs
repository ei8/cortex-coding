using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding
{
    public interface IneurULizer
    {
        Task<Ensemble> neurULizeAsync<TValue>(TValue value, string userId);

        Task<IEnumerable<TValue>> DeneurULizeAsync<TValue>(Ensemble value, string userId)
            where TValue : class, new();
    }
}