using ei8.Cortex.Coding.Model.Properties;
using System;

namespace ei8.Cortex.Coding.Model.Versioning
{
    /// <summary>
    /// Represents the state of a data element at a particular point in time.
    /// </summary>
    public class Snapshot
    {
        /// <summary>
        /// Gets or sets the Id of the Snapshot.
        /// </summary>
        [neurULNeuronProperty]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the Id of the actual Snapshot.
        /// </summary>
        [neurULClass]
        public Guid ValueId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Operation that triggered the Snapshot.
        /// </summary>
        [neurULClass]
        public Guid OperationId { get; set; }
    }
}
