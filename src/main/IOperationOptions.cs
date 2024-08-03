namespace ei8.Cortex.Coding
{
    public interface IOperationOptions<T>
        where T : struct, System.Enum
    {
        T Mode { get; }
    }
}
