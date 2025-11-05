using System;

namespace ei8.Cortex.Coding.Model
{
    /// <summary>
    /// Represents the base class for created Instances. 
    /// </summary>
    public abstract class CreatedInstanceBase : InstanceBase
    {
        /// <summary>
        /// Gets or sets the creation timestamp.
        /// </summary>
        public DateTimeOffset? CreationTimestamp { get; set; }
    }
}
