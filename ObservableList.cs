using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TarLib {
    // TODO: Add observable dictionary
    public class ObservableList<TVariableType> : IList<TVariableType>, IReadOnlyList<TVariableType> {
        private List<TVariableType> items;

        public ObservableList() {
            items = new List<TVariableType>();
        }

        public event EventHandler OnChange;
        public event EventHandler<(int index, TVariableType oldValue, TVariableType newValue)> OnItemChange;
        public event EventHandler<TVariableType> OnAdd;
        public event EventHandler<TVariableType> OnRemove;
        public event EventHandler OnClear;

        public int Count => items.Count;
        public bool IsReadOnly => ((ICollection<TVariableType>)items).IsReadOnly;

        public TVariableType this[int index] {
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

        public int IndexOf(TVariableType item) {
            return items.IndexOf(item);
        }

        public void Insert(int index, TVariableType item) {
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

        public void Add(TVariableType item) {
            items.Add(item);
            OnAdd?.Invoke(this, item);
            OnChange?.Invoke(this, default);
            
        }

        public void AddRange(IEnumerable<TVariableType> collection) {
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

        public bool Contains(TVariableType item) {
            return items.Contains(item);
        }

        public void CopyTo(TVariableType[] array, int arrayIndex) {
            items.CopyTo(array, arrayIndex);
            OnChange?.Invoke(this, default);
        }

        public bool Remove(TVariableType item) {
            var results = items.Remove(item);
            if(results) {
                OnRemove?.Invoke(this, item);
                OnChange?.Invoke(this, default);
            }
            return results;
        }

        public List<TVariableType> ToList() {
            return items.ToList();
        }

        public IEnumerator<TVariableType> GetEnumerator() {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)items).GetEnumerator();
        }
    }
}
