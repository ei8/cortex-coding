﻿using ei8.Cortex.Library.Common;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding
{
    public interface INetworkRepository
    { 
        Task<QueryResult> GetByQueryAsync(NeuronQuery query);

        Task<QueryResult> GetByQueryAsync(NeuronQuery query, bool restrictQueryResultCount);

        Task<QueryResult> GetByQueryAsync(NeuronQuery query, string userId);

        Task<QueryResult> GetByQueryAsync(NeuronQuery query, string userId, bool restrictQueryResultCount);
    }
}
