namespace ei8.Cortex.Coding.Properties.Neuron
{
    public class UrlProperty : INeuronProperty<string>
    {
        public UrlProperty(string value, string name)
        {
            this.Value = value;
            this.Name = name;
        }

        public string Value { get; }

        public string Name { get; }

        public object GetValue() => this.Value;
    }
}
