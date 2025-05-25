namespace ei8.Cortex.Coding
{
    public class neurULizationResult<T>
    {
        public neurULizationResult(
            bool success,
            Neuron instanceNeuron,
            T result
        )
        {
            this.Success = success;
            this.InstanceNeuron = instanceNeuron;
            this.Result = result;
        }

        public bool Success { get; }

        public Neuron InstanceNeuron { get; }

        public T Result { get; private set; }
    }
}
