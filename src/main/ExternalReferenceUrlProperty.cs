namespace ei8.Cortex.Coding
{
    public class ExternalReferenceUrlProperty : INeuronProperty<string>
    {
        public ExternalReferenceUrlProperty(string value)
        {
            this.Value = value;
        }

        public string Value { get; private set; }

        public object GetValue() => this.Value;
    }
}
