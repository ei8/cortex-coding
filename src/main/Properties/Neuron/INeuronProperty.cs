namespace ei8.Cortex.Coding.Properties.Neuron
{
    public interface INeuronProperty
    {
        string Name { get; }

        object GetValue();
    }

    public interface INeuronProperty<TValue> : INeuronProperty
    {
        TValue Value { get; }
    }
}
