namespace ei8.Cortex.Coding.Properties.Neuron
{
    public class RegionTagProperty : INeuronProperty<string>
    {
        public RegionTagProperty(string value, string name)
        {
            this.Value = value;
            this.Name = name;
        }

        public string Value { get; }

        public string Name { get; }

        public object GetValue() => this.Value;
    }
}
