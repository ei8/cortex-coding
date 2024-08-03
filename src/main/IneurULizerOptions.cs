namespace ei8.Cortex.Coding
{
    public interface IneurULizerOptions<TOperationOptions, TMode>
        where TOperationOptions : IOperationOptions<TMode>
        where TMode : struct, System.Enum
    {
        TOperationOptions OperationOptions { get; }
    }
}
