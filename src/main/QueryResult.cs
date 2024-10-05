using System;

namespace ei8.Cortex.Coding
{
    public class QueryResult
    {
        public QueryResult(Ensemble ensemble, Guid userNeuronId)
        {
            this.Ensemble = ensemble;
            this.UserNeuronId = userNeuronId;
        }

        public Ensemble Ensemble { get; set; }

        public Guid UserNeuronId { get; set; }
    }
}
