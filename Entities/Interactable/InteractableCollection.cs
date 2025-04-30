using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TarLib.Primitives;

namespace TarLib.Entities.Interactable {
    public class InteractableCollection : InteractableCollection<IInteractableEntity> {
        
    }

    public class InteractableCollection<TInteractableType> : ISet<TInteractableType>, IReadOnlyCollection<TInteractableType>, IEnumerable<TInteractableType>
        where TInteractableType : IInteractableEntity {

        private HashSet<TInteractableType> items = new HashSet<TInteractableType>();

        public IInteractableEntity GetAt(Vector2 position) {
            foreach (var entity in this) {
                if (entity.IsAt(position)) {
                    return entity.GetElements();
                }
            }
            return null;
        }

        public InteractableCollection GetBetween(RectanglePrimitive selection) {
            var entities = new InteractableCollection();
            foreach (var entity in this) {
                if(entity.IsBetween(selection)) {
                    var elements = entity.GetElements();
                    if (elements is MultipleInteractableEntities multipleEntities) {
                        foreach(var element in multipleEntities.Interactables) {
                            entities.Add(element);
                        }
                    } else {
                        entities.Add(elements);
                    }
                }
            }
            return entities;
        }

        public void RemoveAll(Predicate<TInteractableType> p) {
            var newSet = items.ToList();
            newSet.RemoveAll(p);
            items = newSet.ToHashSet();
        }

        public int Count => ((ICollection<TInteractableType>)items).Count;
        public bool IsReadOnly => ((ICollection<TInteractableType>)items).IsReadOnly;

        public bool Add(TInteractableType item) => items.Add(item);
        public void Clear() => items.Clear();
        public bool Contains(TInteractableType item) => items.Contains(item);
        public void CopyTo(TInteractableType[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);
        public void ExceptWith(IEnumerable<TInteractableType> other) => items.ExceptWith(other);
        public IEnumerator<TInteractableType> GetEnumerator() => items.GetEnumerator();
        public void IntersectWith(IEnumerable<TInteractableType> other) => items.IntersectWith(other);
        public bool IsProperSubsetOf(IEnumerable<TInteractableType> other) => items.IsProperSubsetOf(other);
        public bool IsProperSupersetOf(IEnumerable<TInteractableType> other) => items.IsProperSupersetOf(other);
        public bool IsSubsetOf(IEnumerable<TInteractableType> other) => items.IsSubsetOf(other);
        public bool IsSupersetOf(IEnumerable<TInteractableType> other) => items.IsSupersetOf(other);
        public bool Overlaps(IEnumerable<TInteractableType> other) => items.Overlaps(other);
        public bool Remove(TInteractableType item) => items.Remove(item);
        public bool SetEquals(IEnumerable<TInteractableType> other) => items.SetEquals(other);
        public void SymmetricExceptWith(IEnumerable<TInteractableType> other) => items.SymmetricExceptWith(other);
        public void UnionWith(IEnumerable<TInteractableType> other) => items.UnionWith(other);
        void ICollection<TInteractableType>.Add(TInteractableType item) => items.Add(item);
        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();
    }
}
