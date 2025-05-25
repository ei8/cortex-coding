using neurUL.Common.Domain.Model;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ei8.Cortex.Coding
{
    public static class NetworkExtensions
    {
        public static void ValidateIds(this Network value, IEnumerable<Guid> ids)
        {
            var missingIds = ids.Where(id => !value.TryGetById(id, out Neuron result));

            AssertionConcern.AssertStateTrue(
                !missingIds.Any(),
                $"Failed getting Neurons with IDs: " +
                $"'{string.Join(", ", missingIds)}'."
            );
        }
    }
}
