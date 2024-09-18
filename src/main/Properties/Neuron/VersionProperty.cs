namespace ei8.Cortex.Coding.Properties.Neuron
{
    public class VersionProperty : INeuronProperty<int>
    {
        public VersionProperty(int value, string name)
        {
            this.Value = value;
            this.Name = name;
        }
        public int Value { get; }

        public string Name { get; }

        public object GetValue() => this.Value;
    }
}
