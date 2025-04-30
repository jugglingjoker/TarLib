using System;
using System.Collections;
using System.Collections.Generic;

namespace TarLib.States {
    public class MenuBlockStyleTypeList : ICollection<MenuBlockStyleType> {

        private readonly List<MenuBlockStyleType> types = new List<MenuBlockStyleType>();

        public MenuBlockStyleTypeList() {

        }

        public MenuBlockStyleTypeList(params MenuBlockStyleType[] types) {
            AddRange(types);
        }

        public int Count => types.Count;
        public bool IsReadOnly => ((ICollection<MenuBlockStyleType>)types).IsReadOnly;

        public event EventHandler OnChange;

        public void Add(MenuBlockStyleType item) {
            types.Add(item);
            OnChange?.Invoke(this, null);
        }

        public void AddRange(IEnumerable<MenuBlockStyleType> collection) {
            types.AddRange(collection);
            OnChange?.Invoke(this, null);
        }

        public void Clear() {
            types.Clear();
            OnChange?.Invoke(this, null);
        }

        public bool Contains(MenuBlockStyleType item) {
            return types.Contains(item);
        }

        public void CopyTo(MenuBlockStyleType[] array, int arrayIndex) {
            types.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MenuBlockStyleType> GetEnumerator() {
            return types.GetEnumerator();
        }

        public void Insert(int index, MenuBlockStyleType item) {
            types.Insert(index, item);
        }

        public bool Remove(MenuBlockStyleType item) {
            return types.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return types.GetEnumerator();
        }

        public static implicit operator MenuBlockStyleTypeList(MenuBlockStyleType type) {
            return new MenuBlockStyleTypeList() {
                type
            };
        }

        public static implicit operator MenuBlockStyleTypeList(MenuBlockStyleType[] types) {
            return new MenuBlockStyleTypeList(types);
        }

        public static MenuBlockStyleTypeList operator +(MenuBlockStyleTypeList list, MenuBlockStyleType type) {
            list.Add(type);
            return list;
        }

        public static MenuBlockStyleTypeList operator +(MenuBlockStyleType type, MenuBlockStyleTypeList list) {
            list.Insert(0, type);
            return list;
        }
    }
}
