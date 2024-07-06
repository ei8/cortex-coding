using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding
{
    public enum NeurotransmitterEffect
    {
        Inhibit = -1,
        NotSet,
        Excite
    }
    public static class Constants
    {
        public const string TypeNamePropertyNameSeparator = ":";
    }
}
