using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Cortex.Coding
{
    /// <summary>
    /// Defines the Neuron property that is used to match Neurons in TryParse and GetQueries methods.
    /// </summary>
    public enum ValueMatchBy
    {
        NotSet,
        Id,
        Tag
    }

    public enum ReadMode
    {
        NoSet,
        First,
        All
    }
    public enum WriteMode
    {
        NotSet,
        Create,
        Update
    }

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
