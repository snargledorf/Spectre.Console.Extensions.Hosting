using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Spectre.Console.Extensions.Hosting.Internal
{
    internal static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
        {
#if NETSTANDARD2_0
            return dictionary.TryGetValue(key, out TValue value) ? value : default;
#else
            return CollectionExtensions.GetValueOrDefault(dictionary, key);
#endif
        }

        public static ReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }
    }
}