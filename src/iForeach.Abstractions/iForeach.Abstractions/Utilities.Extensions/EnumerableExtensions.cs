using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace org.iForeach
{
    public static partial class EnumerableExtensions
    {
        public static Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> body)
        {
            var partitionCount = System.Environment.ProcessorCount;

            return Task.WhenAll(Partitioner.Create(source)
                                           .GetPartitions(partitionCount)
                                           .Select(partition => Task.Run(async delegate {
                                               using var tmp = partition;
                                               while (tmp.MoveNext())
                                               {
                                                   await body(tmp.Current);
                                               }
                                           })));
        }

        public static ICollection<TItem> AddRange<TItem>(this ICollection<TItem> collection, IEnumerable<TItem> items)
        {
            switch (collection)
            {
                case List<TItem> list:
                    list.AddRange(items);
                    break;
                default:
                    foreach (var item in items)
                    {
                        collection.Add(item);
                    }
                    break;
            }
            return collection;
        }

        public static TCollection AddRange<TItem, TCollection>(this TCollection collection, IEnumerable<TItem> items)
            where TCollection : ICollection<TItem>
        {
            return (TCollection)collection.AddRange<TItem>(items);
        }

        public static Task<ICollection<TItem>> AddRangeAsync<TItem>(this ICollection<TItem> collection, IEnumerable<TItem> items)
        {
            return Task.Run(() => collection.AddRange(items));
        }

        public static Task<TCollection> AddRangeAsync<TItem, TCollection>(this TCollection collection, IEnumerable<TItem> items)
            where TCollection : ICollection<TItem>
        {
            return Task.Run(() => collection.AddRange(items));
        }
    }

}