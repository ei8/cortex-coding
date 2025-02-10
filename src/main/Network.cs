using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding
{
    public class Network
    {
        private readonly IDictionary<Guid, INetworkItem> itemsDictionary;

        public Network() : this(new Dictionary<Guid, INetworkItem>())
        {
        }

        public Network(IDictionary<Guid, INetworkItem> itemsDictionary) =>
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

        public void AddReplace(INetworkItem item)
        {
            bool replacing = this.itemsDictionary.TryGetValue(item.Id, out INetworkItem oldItem);
            if (replacing)
                Network.ValidateItemReplacementType(item, oldItem);

            Network.AddReplaceCore(item, this.itemsDictionary, replacing);
        }

        private static void AddReplaceCore(INetworkItem item, IDictionary<Guid, INetworkItem> itemsDictionary, bool replacing)
        {
            if (replacing)
                itemsDictionary.Remove(item.Id);

            itemsDictionary.Add(item.Id, item);
        }

        public TNetworkItem AddOrGetIfExists<TNetworkItem>(TNetworkItem value, bool replaceIfExists = false)
            where TNetworkItem : INetworkItem
        {
            TNetworkItem result;
            // if not found in network
            if (!this.TryGetById(value.Id, out result) || replaceIfExists)
            {
                this.AddReplace(value);
                result = value;
            }
            return result;
        }

        private static void ValidateItemReplacementType(INetworkItem newItem, INetworkItem oldItem)
        {
            AssertionConcern.AssertArgumentValid(
                t => t.GetType() == oldItem.GetType(),
                newItem,
                "Item to be replaced must be of the same type as the specified Item.",
                nameof(newItem)
                );
        }

        public void AddReplaceItems(Network network)
        {
            var commonItemsInNewDictionary = network.itemsDictionary.Where(item => itemsDictionary.ContainsKey(item.Key)).ToList();
            // validate all common items in specified network
            commonItemsInNewDictionary.ForEach(ci => Network.ValidateItemReplacementType(ci.Value, itemsDictionary[ci.Key]));
            network.itemsDictionary.ToList().ForEach(ni => Network.AddReplaceCore(ni.Value, itemsDictionary, commonItemsInNewDictionary.Contains(ni)));
        }

        public void Remove(Guid id) => this.itemsDictionary.Remove(id);

        public IEnumerable<Terminal> GetDendrites(Guid neuronId) =>
            this.GetItems<Terminal>().Where(t => t.PostsynapticNeuronId == neuronId);

        public IEnumerable<Terminal> GetTerminals(Guid neuronId) =>
            this.GetItems<Terminal>().Where(t => t.PresynapticNeuronId == neuronId);

        public IEnumerable<Neuron> GetPresynapticNeurons(Guid neuronId)
        {
            return this.GetDendrites(neuronId)
                 .Select(t =>
                 {
                     this.TryGetById(t.PresynapticNeuronId, out Neuron result);
                     neurUL.Common.Domain.Model.AssertionConcern.AssertStateTrue(
                         result != null,
                         $"Neuron with specified Presynaptic Neuron Id of '{t.PresynapticNeuronId}' was not found."
                         );
                     return result;
                 });
        }

        public IEnumerable<Neuron> GetPostsynapticNeurons(Guid neuronId)
        {
            var terminals = this.GetTerminals(neuronId);
            return terminals.Select(t =>
                 {
                     neurUL.Common.Domain.Model.AssertionConcern.AssertStateTrue(
                         this.TryGetById(t.PostsynapticNeuronId, out Neuron result),
                         "Neuron with specified Postsynaptic Neuron Id was not found."
                         );
                     return result;
                 });
        }

        public bool AnyTransient() => this.itemsDictionary.Values.Any(i => i.IsTransient);

        // TODO: enable if needed by client code
        // public IDictionary<T, Neuron> GetInterneurons<T>(
        //    Neuron presynaptic, 
        //    IEnumerable<T> ids, params Func<T, Neuron>[] postsynapticRetrievers
        //    )
        //{
        //    Dictionary<T, Neuron> result = new Dictionary<T, Neuron>();

        //    var prePosts = this.GetPostsynapticNeurons(presynaptic.Id);

        //    foreach(T id in ids)
        //    {
        //        IEnumerable<Neuron> postPres = null;
        //        foreach (var pr in postsynapticRetrievers)
        //        {
        //            var post = pr(id);
        //            if (post != null)
        //            {
        //                if (postsynapticRetrievers.First() == pr)
        //                    postPres = this.GetPresynapticNeurons(post.Id);
        //                else
        //                {
        //                    var tempPostPres = this.GetPresynapticNeurons(post.Id);
        //                    postPres = postPres.Intersect(tempPostPres);
        //                }

        //                if (postPres == null || !postPres.Any())
        //                    break;
        //            }
        //        }

        //        Neuron match = null;

        //        if (postPres != null &&
        //            (match = prePosts.Intersect(postPres)
        //            .FirstOrDefault(pp => !result.ContainsValue(pp))
        //            ) != null)
        //            result.Add(id, match);
        //    }

        //    return result;
        //}
    }
}
