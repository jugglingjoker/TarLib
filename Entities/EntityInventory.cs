using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TarLib.Entities {

    public abstract class EntityInventory<TInventoryType> : ReservationCollection<TInventoryType, uint>, IReadOnlyDictionary<TInventoryType, EntityInventory<TInventoryType>.Quantity> {
        private Dictionary<TInventoryType, Quantity> counts = new();

        public event EventHandler OnChange;

        public Quantity this[TInventoryType key] => counts[key];
        public IEnumerable<TInventoryType> Keys => counts.Keys;
        public IEnumerable<Quantity> Values => counts.Values;
        public int Count => counts.Count;
        public bool ContainsKey(TInventoryType key) => counts.ContainsKey(key);
        public IEnumerator<KeyValuePair<TInventoryType, Quantity>> GetEnumerator() => counts.GetEnumerator();
        public bool TryGetValue(TInventoryType key, [MaybeNullWhen(false)] out Quantity value) => counts.TryGetValue(key, out value);
        IEnumerator IEnumerable.GetEnumerator() => counts.GetEnumerator();

        public abstract bool CanRemoveEmptyInventoryItems { get; }

        public EntityInventory() {
            OnAdd += EntityInventory_OnAdd;
            OnReserveAdd += EntityInventory_OnReserveAdd;
            OnCancelAdd += EntityInventory_OnCancelAdd;
            OnRemove += EntityInventory_OnRemove;
            OnReserveRemove += EntityInventory_OnReserveRemove;
            OnCancelRemove += EntityInventory_OnCancelRemove;
        }

        private void EntityInventory_OnReserveAdd(object sender, AddTicket e) {
            if (!counts.ContainsKey(e.Entity)) {
                counts[e.Entity] = new(this, e.Entity);
            }
            OnChange?.Invoke(sender, default);
        }

        private void EntityInventory_OnCancelAdd(object sender, AddTicket e) {
            OnChange?.Invoke(sender, default);
        }

        private void EntityInventory_OnAdd(object sender, AddTicket e) {
            OnChange?.Invoke(sender, default);
        }

        private void EntityInventory_OnReserveRemove(object sender, RemoveTicket e) {
            OnChange?.Invoke(sender, default);
        }

        private void EntityInventory_OnCancelRemove(object sender, RemoveTicket e) {
            OnChange?.Invoke(sender, default);
        }

        private void EntityInventory_OnRemove(object sender, RemoveTicket e) {
            OnChange?.Invoke(sender, default);
        }

        protected override void Add(AddTicket ticket) {
            if (!counts.ContainsKey(ticket.Entity)) {
                counts[ticket.Entity] = new(this, ticket.Entity);
            }
            counts[ticket.Entity].Add(ticket.Config);
        }

        protected override void Remove(RemoveTicket ticket) {
            if (counts.ContainsKey(ticket.Entity)) {
                counts[ticket.Entity].Remove(ticket.Config);
                if (counts[ticket.Entity].All == 0 && CanRemoveEmptyInventoryItems) {
                    counts.Remove(ticket.Entity);
                }
            }
        }

        public override bool CanRemove(TInventoryType entity, uint quantity) {
            return counts.ContainsKey(entity) && counts[entity].Actual <= quantity;
        }

        public class Quantity {
            public Quantity(EntityInventory<TInventoryType> inventory, TInventoryType item) {
                Inventory = inventory;
                Item = item;
            }

            public EntityInventory<TInventoryType> Inventory { get; }
            public TInventoryType Item { get; }

            public uint Actual { get; private set; }
            // TODO: Change to methods inside the inventory object, maintain these numbers based on add and remove processes
            public uint Incoming => (uint)Inventory.AddTickets.Select(ticket => ticket.Entity.Equals(Item) ? (int)ticket.Config : 0).Sum();
            public uint Outgoing => (uint)Inventory.RemoveTickets.Select(ticket => ticket.Entity.Equals(Item) ? (int)ticket.Config : 0).Sum();

            public uint Projected => Actual + Incoming;
            public uint All => Actual + Incoming - Outgoing;
            public uint Available => Actual - Outgoing;

            internal void Add(uint quantity) {
                Actual += quantity;
            }

            internal void Remove(uint quantity) {
                Actual -= quantity;
            }
        }
    }
}
