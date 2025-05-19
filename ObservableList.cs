using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TarLib {

    public class ObservableList<T> : IList<T>, IReadOnlyList<T> {
        private List<T> items;

        public ObservableList() {
            items = new List<T>();
            
        }

        public event EventHandler OnChange;
        public event EventHandler<(int index, T oldValue, T newValue)> OnItemChange;
        public event EventHandler<T> OnAdd;
        public event EventHandler<T> OnRemove;
        public event EventHandler OnClear;

        public int Count => items.Count;
        public bool IsReadOnly => ((ICollection<T>)items).IsReadOnly;

        public T this[int index] {
            get => items[index];
            set {
                if(!(items[index]?.Equals(value) ?? value == null)) {
                    var oldValue = items[index];
                    items[index] = value;
                    OnItemChange?.Invoke(this, (index, oldValue, value));
                    OnChange?.Invoke(this, default);
                }
            }
        }

        public int IndexOf(T item) {
            return items.IndexOf(item);
        }

        public void Insert(int index, T item) {
            var oldValue = index < Count && index > 0 ? items[index] : default;
            items.Insert(index, item);
            OnAdd?.Invoke(this, item);
            OnItemChange?.Invoke(this, (index, oldValue, item));
            OnChange?.Invoke(this, default);
        }

        public void RemoveAt(int index) {
            var item = items[index];
            items.RemoveAt(index);
            OnRemove?.Invoke(this, item);
            OnChange?.Invoke(this, default);
        }

        public void Add(T item) {
            items.Add(item);
            OnAdd?.Invoke(this, item);
            OnChange?.Invoke(this, default);
            
        }

        public void AddRange(IEnumerable<T> collection) {
            items.AddRange(collection);
            foreach (var variable in collection) {
                OnAdd?.Invoke(this, variable);
            }
            OnChange?.Invoke(this, default);
        }

        public void Clear() {
            items.Clear();
            OnClear?.Invoke(this, default);
            OnChange?.Invoke(this, default);
        }

        public bool Contains(T item) {
            return items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            items.CopyTo(array, arrayIndex);
            OnChange?.Invoke(this, default);
        }

        public bool Remove(T item) {
            var results = items.Remove(item);
            if(results) {
                OnRemove?.Invoke(this, item);
                OnChange?.Invoke(this, default);
            }
            return results;
        }

        public List<T> ToList() {
            return items.ToList();
        }

        public IEnumerator<T> GetEnumerator() {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)items).GetEnumerator();
        }
    }
}
