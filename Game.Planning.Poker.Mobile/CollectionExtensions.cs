using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Game.Planning.Poker.Mobile
{
    public static class CollectionExtensions
    {
        public static void Update<T>(this ObservableCollection<T> collection, IEnumerable<T> enumerable)
        {
            var elements = enumerable.ToList();
            var array = collection.ToArray();
            for (var index = 0; index < array.Length; index++)
            {
                if (!elements.Contains(array[index]))
                {
                    collection.RemoveAt(index);
                }
            }

            for (var i = 0; i < elements.Count; i++)
            {
                var element = elements[i];
                var oldIndex = collection.IndexOf(element);

                if (oldIndex != i)
                {
                    if (oldIndex == -1)
                    {
                        collection.Insert(i, element);
                    }
                    else
                    {
                        collection.Move(oldIndex, i);
                    }
                }
            }
        }
    }
}