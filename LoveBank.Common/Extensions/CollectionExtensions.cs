using System.Collections.Generic;

namespace LoveBank.Common
{
    public static class CollectionExtensions
    {
        public  static void AddRange<T>(this ICollection<T> instance,IEnumerable<T> collection  )
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotNull(collection, "collection");

            foreach (T item in collection)
            {
                instance.Add(item);
            }
        }

    }
}
