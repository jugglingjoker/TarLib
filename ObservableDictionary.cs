using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TarLib {

    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue> {
        private Dictionary<TKey, TValue> items = new();

        public TValue this[TKey key] { 
            get => items[key];
            set => Add(key, value);
        }

        public ICollection<TKey> Keys => items.Keys;
        public ICollection<TValue> Values => items.Values;
        public int Count => items.Count;
        public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)items).IsReadOnly;
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => items.Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => items.Values;

        public event EventHandler OnChange;
        public event EventHandler<(TKey key, TValue oldValue, TValue newValue)> OnItemChange;
        public event EventHandler<KeyValuePair<TKey, TValue>> OnAdd;
        public event EventHandler<KeyValuePair<TKey, TValue>> OnRemove;
        public event EventHandler OnClear;

        public void Add(TKey key, TValue value) {
            if(items.ContainsKey(key)) {
                if(!items[key].Equals(value)) {
                    var oldValue = items[key];
                    items[key] = value;
                    OnItemChange?.Invoke(this, (key, oldValue, value));
                    OnAdd?.Invoke(this, new(key, value));
                    OnChange?.Invoke(this, default);
                }
            } else {
                items[key] = value;
                OnAdd?.Invoke(this, new(key, value));
                OnChange?.Invoke(this, default);
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item) {
            Add(item.Key, item.Value);
        }

        public void Clear() {
            items.Clear();
            OnClear?.Invoke(this, default);
            OnChange?.Invoke(this, default);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) {
            return items.ContainsKey(item.Key) ? items[item.Key].Equals(item.Value) : false;
        }

        public bool ContainsKey(TKey key) {
            return items.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            ((ICollection<KeyValuePair<TKey, TValue>>)items).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            return items.GetEnumerator();
        }

        public bool Remove(TKey key) {
            if(items.ContainsKey(key)) {
                var value = items[key];
                var response = items.Remove(key);
                OnRemove?.Invoke(this, new(key, value));
                OnChange?.Invoke(this, default);
                return response;
            } else {
                return items.Remove(key);
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) {
            if(items.ContainsKey(item.Key) && items[item.Key].Equals(item.Value)) {
                return Remove(item.Key);
            } else {
                return false;
            }
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) {
            return items.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return items.GetEnumerator();
        }
    }
}
