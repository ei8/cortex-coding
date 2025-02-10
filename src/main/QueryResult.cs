using System;

namespace ei8.Cortex.Coding
{
    public class QueryResult
    {
        public QueryResult(Network network, Guid userNeuronId)
        {
            this.Network = network;
            this.UserNeuronId = userNeuronId;
        }

        public Network Network { get; set; }

        public Guid UserNeuronId { get; set; }
    }
}
