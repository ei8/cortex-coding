using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding
{
    public interface IneurULizer
    {
        Task<Ensemble> neurULizeAsync<TValue, TOptions>(TValue value, TOptions options)
            where TOptions : IneurULizerWriteOptions;

        IEnumerable<TValue> DeneurULize<TValue, TOptions>(Ensemble value, TOptions options)
            where TValue : class, new()
            where TOptions : IneurULizerReadOptions;
    }
}