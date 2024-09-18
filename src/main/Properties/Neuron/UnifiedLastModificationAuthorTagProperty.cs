namespace ei8.Cortex.Coding.Properties.Neuron
{
    public class UnifiedLastModificationAuthorTagProperty : INeuronProperty<string>
    {
        public UnifiedLastModificationAuthorTagProperty(string value, string name)
        {
            this.Value = value;
            this.Name = name;
        }

        public string Value { get; }

        public string Name { get; }

        public object GetValue() => this.Value;
    }
}
