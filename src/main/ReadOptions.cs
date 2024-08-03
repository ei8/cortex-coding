namespace ei8.Cortex.Coding
{
    public class ReadOptions : IReadOptions
    {
        public ReadOptions(ReadMode mode)
        {
            this.Mode = mode;
        }

        public ReadMode Mode { get; }
    }
}
