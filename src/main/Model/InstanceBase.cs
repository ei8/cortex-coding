using ei8.Cortex.Coding.Model.Properties;
using System;

namespace ei8.Cortex.Coding.Model
{
    /// <summary>
    /// Represents the base class for Instances. 
    /// </summary>
    public abstract class InstanceBase
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        [neurULNeuronProperty]
        public Guid Id { get; set; }
    }
}
