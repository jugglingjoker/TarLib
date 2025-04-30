using System;
using System.Collections;
using System.Collections.Generic;

namespace TarLib.States {
    public class MenuBlockStyleTagList : ICollection<string> {

        private List<string> tags = new List<string>();

        public int Count => tags.Count;
        public bool IsReadOnly => ((ICollection<string>)tags).IsReadOnly;

        public event EventHandler OnChange;

        public void Add(string item) {
            tags.Add(item);
            OnChange?.Invoke(this, null);
        }
        
        public void AddRange(IEnumerable<string> collection) {
            tags.AddRange(collection);
            OnChange?.Invoke(this, null);
        }

        public void Clear() {
           tags.Clear();
            OnChange?.Invoke(this, null);
        }
        
        public bool Contains(string item) {
            return tags.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex) {
            tags.CopyTo(array, arrayIndex);
        }
        
        public IEnumerator<string> GetEnumerator() {
            return tags.GetEnumerator();
        }

        public bool Remove(string item) {
            return tags.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return tags.GetEnumerator();
        }

        public static implicit operator MenuBlockStyleTagList(string tag) {
            return new MenuBlockStyleTagList() {
                tag
            };
        }

        public static implicit operator MenuBlockStyleTagList(string[] tags) {
            var tagList = new MenuBlockStyleTagList();
            tagList.AddRange(tags);
            return tagList;
        }


    }
}
