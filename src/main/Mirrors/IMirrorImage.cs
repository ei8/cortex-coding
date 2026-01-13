using System;

namespace ei8.Cortex.Coding.Mirrors
{
    /// <summary>
    /// Provides functionality for Mirror Images.
    /// </summary>
    public interface IMirrorImage
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Gets or sets the Mirror.
        /// </summary>
        MirrorInfo Mirror { get; set; }
    }
}
