namespace ei8.Cortex.Coding
{
    public class WriteOptions : IWriteOptions
    {
        public WriteOptions(WriteMode mode)
        {
            this.Mode = mode;
        }

        public WriteMode Mode { get; }
    }
}
