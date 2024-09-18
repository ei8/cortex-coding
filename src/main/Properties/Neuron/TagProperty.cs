namespace ei8.Cortex.Coding.Properties.Neuron
{
    public class TagProperty : INeuronProperty<string>
    {
        public TagProperty(string value, string name)
        {
            this.Value = value;
            this.Name = name;
        }

        public string Value { get; private set; }

        public string Name { get; }

        public object GetValue() => this.Value;
    }
}
