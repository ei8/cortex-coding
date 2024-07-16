namespace ei8.Cortex.Coding
{
    public interface INeuronProperty
    {
        object GetValue();
    }

    public interface INeuronProperty<TValue> : INeuronProperty
    {
        TValue Value { get; }
    }
}
