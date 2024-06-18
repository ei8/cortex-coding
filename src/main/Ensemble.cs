using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding
{
    public class Ensemble
    {
        private readonly IDictionary<Guid, IEnsembleItem> itemsDictionary;

        public Ensemble() : this(new Dictionary<Guid, IEnsembleItem>())
        {
        }

        public Ensemble(IDictionary<Guid, IEnsembleItem> itemsDictionary) =>
            this.itemsDictionary = itemsDictionary;

        public bool TryGetById<T>(
                Guid id,
                out T result
            )
            where T : IEnsembleItem
        {
            bool bResult = false;
            result = default;

            if (itemsDictionary.TryGetValue(id, out IEnsembleItem tryResult))
            {
                result = (T)tryResult;
                bResult = true;
            }

            return bResult;
        }

        public IEnumerable<T> GetItems<T>()
            where T : IEnsembleItem
            => itemsDictionary.Values.OfType<T>();

        public void AddReplace(IEnsembleItem item)
        {
            bool replacing = itemsDictionary.TryGetValue(item.Id, out IEnsembleItem oldItem);
            if (replacing)
                ValidateItemReplacementType(item, oldItem);

            AddReplaceCore(item, itemsDictionary, replacing);
        }

        private static void AddReplaceCore(IEnsembleItem item, IDictionary<Guid, IEnsembleItem> itemsDictionary, bool replacing)
        {
            if (replacing)
                itemsDictionary.Remove(item.Id);

            itemsDictionary.Add(item.Id, item);
        }

        public TEnsembleItem Obtain<TEnsembleItem>(TEnsembleItem value)
            where TEnsembleItem : IEnsembleItem
        {
            TEnsembleItem result = default;
            // if not found in ensemble
            if (!TryGetById(value.Id, out result))
            {
                AddReplace(value);
                result = value;
            }
            return result;
        }

        private static void ValidateItemReplacementType(IEnsembleItem newItem, IEnsembleItem oldItem)
        {
            AssertionConcern.AssertArgumentValid(
                t => t.GetType() == oldItem.GetType(),
                newItem,
                "Item to be replaced must be of the same type as the specified Item.",
                nameof(newItem)
                );
        }

        public void AddReplaceItems(Ensemble ensemble)
        {
            var commonItemsInNewDictionary = ensemble.itemsDictionary.Where(item => itemsDictionary.ContainsKey(item.Key)).ToList();
            // validate all common items in specified ensemble
            commonItemsInNewDictionary.ForEach(ci => ValidateItemReplacementType(ci.Value, itemsDictionary[ci.Key]));
            ensemble.itemsDictionary.ToList().ForEach(ni => AddReplaceCore(ni.Value, itemsDictionary, commonItemsInNewDictionary.Contains(ni)));
        }

        public IEnumerable<Terminal> GetDendrites(Guid neuronId) =>
            this.GetItems<Terminal>().Where(t => t.PostsynapticNeuronId == neuronId);

        public IEnumerable<Terminal> GetTerminals(Guid neuronId) =>
            this.GetItems<Terminal>().Where(t => t.PresynapticNeuronId == neuronId);

        public IEnumerable<Neuron> GetPresynapticNeurons(Guid neuronId) =>
            this.GetDendrites(neuronId)
                 .Select(t => {
                     neurUL.Common.Domain.Model.AssertionConcern.AssertStateTrue(
                         this.TryGetById(t.PresynapticNeuronId, out Neuron result),
                         "Neuron with specified Presynaptic Neuron Id was not found."
                         );
                     return result;
                 });
    }
}
