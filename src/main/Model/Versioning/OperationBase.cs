using ei8.Cortex.Coding.Model.Properties;
using System;

namespace ei8.Cortex.Coding.Model.Versioning
{
    /// <summary>
    /// Represents the base class for Operation Events. Used by instances to log Aggregate-scope events 
    /// that occur outside of a granny neuron and thus may not be recorded in its Timestamps.
    /// </summary>
    public abstract class OperationBase
    {
        /// <summary>
        /// Gets or sets the Id of the Operation.
        /// </summary
        [neurULNeuronProperty]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the Id of the data element the Operation was applied to.
        /// </summary>
        [neurULClass]
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the point in time in which the Operation occurred.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }
    }
}
