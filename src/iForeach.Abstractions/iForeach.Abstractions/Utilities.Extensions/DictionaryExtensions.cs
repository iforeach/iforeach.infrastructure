using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace org.iForeach
{
    public static partial class DictionaryExtensions
    {
        /// <summary>
        /// <seealso cref="ConcurrentDictionary{TKey, TValue}.GetOrAdd(TKey, Func{TKey, TValue})"/>
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <param name="actionAfterAded">新增完成后的后续处理</param>
        /// <returns></returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TKey, TValue> valueFactory, Action<TKey, TValue> actionAfterAded = null)
        {
            if(source.TryGetValue(key, out var value))
            {
                return value;
            }
            lock(source)
            {
                if (!source.TryGetValue(key, out value))
                {
                    source[key] = value = valueFactory(key);
                    actionAfterAded?.Invoke(key, value);
                }
                return value;
            }
        }
    }

}