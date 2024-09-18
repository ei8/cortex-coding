namespace ei8.Cortex.Coding.Properties.Neuron
{
    public class ExternalReferenceUrlProperty : INeuronProperty<string>
    {
        public ExternalReferenceUrlProperty(string value, string name)
        {
            this.Value = value;
            this.Name = name;
        }

        public string Value { get; private set; }

        public string Name { get; }

        public object GetValue() => this.Value;
    }
}
