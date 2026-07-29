using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding
{
    public class Network : ReadOnlyNetwork
    {
        public Network() : this(new Dictionary<Guid, INetworkItem>())
        {
        }

        public Network(IDictionary<Guid, INetworkItem> itemsDictionary) : base(itemsDictionary) 
        { 
        }

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

        public void AddReplaceItems(ReadOnlyNetwork network)
        {
            var commonItemsInNewDictionary = network.GetItems().Where(item => this.itemsDictionary.ContainsKey(item.Id)).ToList();
            // validate all common items in specified network
            commonItemsInNewDictionary.ForEach(ci => Network.ValidateItemReplacementType(ci, itemsDictionary[ci.Id]));
            network.GetItems().ToList().ForEach(ni => Network.AddReplaceCore(ni, itemsDictionary, commonItemsInNewDictionary.Contains(ni)));
        }

        public void Remove(Guid id) => this.itemsDictionary.Remove(id);

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

        //public IEnumerable<Tuple<int, Neuron>> GetFarthestPresynaptics(Guid id)
        //{
        //    var result = Enumerable.Empty<Tuple<int, Neuron>>();

        //    var log = Network.GetPresynapticsWithLevels(
        //        this,
        //        id
        //    );

        //    if (log.Any())
        //        result = log.Where(l => l.Item1 == log.Max(l2 => l2.Item1));

        //    return result;
        //}

        //private static IEnumerable<Tuple<int, Neuron>> GetPresynapticsWithLevels(
        //    Network value,
        //    Guid id,
        //    int level = 0
        //)
        //{
        //    var log = new List<Tuple<int, Neuron>>();

        //    Neuron target = null;
        //    AssertionConcern.AssertArgumentValid(
        //        i => value.TryGetById<Neuron>(i, out target),
        //        id,
        //        $"Neuron with specified '{id}' not found.",
        //        nameof(id)
        //    );

        //    foreach (var pre in value.GetPresynapticNeurons(target.Id))
        //    {
        //        log.Add(Tuple.Create(level + 1, pre));
        //        log.AddRange(
        //            Network.GetPresynapticsWithLevels(
        //                value,
        //                pre.Id,
        //                level + 1
        //            )
        //        );
        //    }

        //    return log;
        //}
    }
}
