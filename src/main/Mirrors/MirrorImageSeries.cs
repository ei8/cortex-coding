using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ei8.Cortex.Coding.Mirrors
{
    /// <summary>
    /// Represents series of Mirror Images.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MirrorImageSeries<T> : KeyedCollection<Guid, T>, IMirrorImageSeries<T> where T : IMirrorImage
    {
        /// <summary>
        /// Constructs a MirrorImageSeries.
        /// </summary>
        public MirrorImageSeries() : this(Enumerable.Empty<T>())
        {
        }

        /// <summary>
        /// Constructs a MirrorImageSeries based on the specified series.
        /// </summary>
        /// <param name="series"></param>
        public MirrorImageSeries(IEnumerable<T> series)
        {
            AssertionConcern.AssertArgumentNotNull(series, nameof(series));

            // ensure all items are related to each other
            for (int i = 0; i < series.Count() - 1; i++)
            {
                MirrorImageSeries<T>.ValidateSeriesPair(
                    series.ElementAt(i), 
                    series.ElementAt(i + 1)
                );
            }

            foreach (var s in series)
                this.Add(s);
        }

        protected override Guid GetKeyForItem(T item) => item.Id;

        private static void ValidateSeriesPair(T precedingItem, T subsequentItem)
        {
            AssertionConcern.AssertArgumentNotNull(
                precedingItem.Mirror,
                $"{nameof(precedingItem)}.Mirror"
            );
            AssertionConcern.AssertArgumentNotNull(precedingItem, nameof(precedingItem));
            AssertionConcern.AssertArgumentNotNull(subsequentItem, nameof(subsequentItem));
            AssertionConcern.AssertArgumentValid(
                s => s.Mirror.Url == subsequentItem.Url,
                precedingItem,
                $"Series item with ID:MirrorURL '{precedingItem.Id}:{precedingItem.Mirror.Url}' is not equal to subsequent item ID:URL '{subsequentItem.Id}:{subsequentItem.Url}'.",
                nameof(precedingItem)
            );
        }

        /// <summary>
        /// Gets the ID.
        /// </summary>
        public Guid? Id => this.Items.FirstOrDefault()?.Id;

        protected override void InsertItem(int index, T item)
        {
            // when inserting an item...
            if (index <  this.Count)
                // ...ensure new item is correctly related to existing item at specified index
                MirrorImageSeries<T>.ValidateSeriesPair(
                    item,
                    this[index]
                );
            // when appending item...
            else if (this.Count > 0)
                // ensure new item is correctly related to preceding item
                MirrorImageSeries<T>.ValidateSeriesPair(
                    this[index - 1],
                    item
                );

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            // when removing an item, ensure specified item is not in between two other items
            AssertionConcern.AssertArgumentValid(
                i => i == 0 || i == this.Items.Count - 1,
                index,
                "Only items at the beginning and at the end of the series can be removed.",
                nameof(index)
            );

            base.RemoveItem(index);
        }
    }
}