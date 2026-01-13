using System;

namespace ei8.Cortex.Coding.Mirrors
{
    /// <summary>
    /// Represents the Mirror data of a Neuron.
    /// </summary>
    public class MirrorInfo
    {
        /// <summary>
        /// Constructs a MirrorInfo with no URL.
        /// </summary>
        public MirrorInfo() : this(null) { }

        /// <summary>
        /// Constructs a MirrorInfo with a specified URL.
        /// </summary>
        /// <param name="url"></param>
        public MirrorInfo(string url)
        {
            this.Url = url;
            this.IsValid = null;
            this.ValidationTimestamp = null;
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets an indication whether the URL is valid.
        /// </summary>
        public bool? IsValid { get; set; }

        /// <summary>
        /// Gets or sets the timestamp at which the URL was last validated.
        /// </summary>
        public DateTimeOffset? ValidationTimestamp { get; set; }
    }
}
