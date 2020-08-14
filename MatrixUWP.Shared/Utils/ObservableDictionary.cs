#nullable enable
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace MatrixUWP.Shared.Utils
{
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>
    {
        private readonly ObservableCollection<TKey> keys = new ObservableCollection<TKey>();
        private readonly ObservableCollection<TValue> values = new ObservableCollection<TValue>();
        private readonly Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
        public TValue this[TKey key]
        {
            get => dictionary[key];
            set => Add(key, value);
        }

        public ICollection<TKey> Keys => keys;

        public ICollection<TValue> Values => values;

        public int Count => dictionary.Count;

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                values.Remove(dictionary[key]);
                values.Add(value);
                dictionary[key] = value;
            }
            else
            {
                keys.Add(key);
                values.Add(value);
                dictionary[key] = value;
            }
        }
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            var (key, value) = item;
            Add(key, value);
        }
        public void Clear()
        {
            keys.Clear();
            values.Clear();
            dictionary.Clear();
        }
        public bool Contains(KeyValuePair<TKey, TValue> item) => dictionary.Contains(item);
        public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            var count = 0;
            foreach (var item in dictionary)
            {
                array[arrayIndex + count] = item;
                count++;
            }
        }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dictionary.GetEnumerator();
        public bool Remove(TKey key)
        {
            var result = dictionary.Remove(key, out var value);
            if (result)
            {
                keys.Remove(key);
                values.Remove(value);
            }
            return result;
        }
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var key = item.Key;
            return Remove(key);
        }
        public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);
        IEnumerator IEnumerable.GetEnumerator() => dictionary.GetEnumerator();
    }
}
