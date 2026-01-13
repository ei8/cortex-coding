using ei8.Cortex.Coding.Mirrors;
using System;

namespace ei8.Cortex.Coding.Test.Mirrors
{
    public class MockMirrorImage : IMirrorImage
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public MirrorInfo Mirror { get; set; }
    }
}
