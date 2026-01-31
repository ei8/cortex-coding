using System;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.Mirrors
{
    /// <summary>
    /// Provides functionality for series of MirrorImages.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMirrorImageSeries<T> : ICollection<T> where T : IMirrorImage
    {
        /// <summary>
        /// Gets the ID.
        /// </summary>
        Guid? Id { get; }

        /// <summary>
        /// Gets the first item in the series.
        /// </summary>
        T First { get; }
    }
}
