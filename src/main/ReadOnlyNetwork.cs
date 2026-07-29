using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding
{
    public class ReadOnlyNetwork
    {
        private const string NotFoundMessage = "Neuron with specified {0} Neuron Id of '{1]' was not found.";
        protected readonly IDictionary<Guid, INetworkItem> itemsDictionary;

        public ReadOnlyNetwork() : this(new Dictionary<Guid, INetworkItem>())
        {
        }

        public ReadOnlyNetwork(IDictionary<Guid, INetworkItem> itemsDictionary) =>
            this.itemsDictionary = itemsDictionary;

        public bool TryGetById<T>(
                Guid id,
                out T result
            )
            where T : INetworkItem
        {
            bool bResult = false;
            result = default;

            if (itemsDictionary.TryGetValue(id, out INetworkItem tryResult))
            {
                result = (T)tryResult;
                bResult = true;
            }

            return bResult;
        }

        public IEnumerable<INetworkItem> GetItems() => this.GetItems<INetworkItem>();

        public IEnumerable<T> GetItems<T>()
            where T : INetworkItem
            => itemsDictionary.Values.OfType<T>();

        public IEnumerable<Terminal> GetPresynapticTerminals(Guid neuronId) =>
            this.GetItems<Terminal>().Where(t => t.PostsynapticNeuronId == neuronId);

        public IEnumerable<Terminal> GetPostsynapticTerminals(Guid neuronId) =>
            this.GetItems<Terminal>().Where(t => t.PresynapticNeuronId == neuronId);

        public IEnumerable<Neuron> GetPresynapticNeurons(Guid neuronId)
        {
            return this.GetPresynapticTerminals(neuronId)
                 .Select(t =>
                 {
                     this.TryGetById(t.PresynapticNeuronId, out Neuron result);
                     
                     neurUL.Common.Domain.Model.AssertionConcern.AssertStateTrue(
                         result != null,
                         string.Format(ReadOnlyNetwork.NotFoundMessage, "Presynaptic", t.PresynapticNeuronId)
                         );
                     return result;
                 });
        }

        public IEnumerable<Neuron> GetPostsynapticNeurons(Guid neuronId)
        {
            var terminals = this.GetPostsynapticTerminals(neuronId);
            return terminals.Select(t =>
            {
                neurUL.Common.Domain.Model.AssertionConcern.AssertStateTrue(
                    this.TryGetById(t.PostsynapticNeuronId, out Neuron result),
                    string.Format(ReadOnlyNetwork.NotFoundMessage, "Postsynaptic", t.PostsynapticNeuronId)
                    );
                return result;
            });
        }

        public bool AnyTransient() => this.itemsDictionary.Values.Any(i => i.IsTransient);

        public bool TryGetByTag(string tag, out IEnumerable<Neuron> result)
        {
            bool result2 = false;
            result = null;
            var matches = this.GetItems<Neuron>().Where(n => n.Tag == tag);

            if (matches.Any())
            {
                result = matches;
                result2 = true;
            }

            return result2;
        }
    }
}
