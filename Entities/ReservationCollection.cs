using System;
using System.Collections.Generic;

namespace TarLib.Entities {
    public abstract class ReservationCollection<TEntity, TAddTicket, TRemoveTicket, TTicketConfig>
        where TAddTicket : ReservationCollection<TEntity, TAddTicket, TRemoveTicket, TTicketConfig>.IReservationTicket
        where TRemoveTicket : ReservationCollection<TEntity, TAddTicket, TRemoveTicket, TTicketConfig>.IReservationTicket {

        public event EventHandler<TAddTicket> OnReserveAdd;
        public event EventHandler<TAddTicket> OnAdd;
        public event EventHandler<TAddTicket> OnCancelAdd;
        public event EventHandler<TRemoveTicket> OnReserveRemove;
        public event EventHandler<TRemoveTicket> OnRemove;
        public event EventHandler<TRemoveTicket> OnCancelRemove;

        private List<TAddTicket> addTickets = new();
        private List<TRemoveTicket> removeTickets = new();

        public IReadOnlyList<TAddTicket> AddTickets => addTickets;
        public IReadOnlyList<TRemoveTicket> RemoveTickets => removeTickets;

        protected abstract void Add(TAddTicket ticket);
        protected abstract TAddTicket CreateAddReservation(TEntity entity, TTicketConfig config);
        protected abstract void Remove(TRemoveTicket ticket);
        protected abstract TRemoveTicket CreateRemoveReservation(TEntity entity, TTicketConfig config);

        public abstract bool CanAdd(TEntity entity, TTicketConfig config = default);
        public abstract bool CanRemove(TEntity entity, TTicketConfig config = default);

        public TAddTicket ReserveAdd(TEntity entity, TTicketConfig config = default) {
            if(CanAdd(entity, config)) {
                var ticket = CreateAddReservation(entity, config);
                addTickets.Add(ticket);
                OnReserveAdd?.Invoke(this, ticket);
                return ticket;
            } else {
                return default;
            }
        }

        public bool CompleteAdd(TAddTicket ticket) {
            if(addTickets.Contains(ticket)) {
                addTickets.Remove(ticket);
                Add(ticket);
                OnAdd?.Invoke(this, ticket);
                return true;
            } else {
                return false;
            }
        }

        public bool CancelAdd(TAddTicket ticket) {
            if (addTickets.Contains(ticket)) {
                addTickets.Remove(ticket);
                OnCancelAdd?.Invoke(this, ticket);
                return true;
            } else {
                return false;
            }
        }

        public TRemoveTicket ReserveRemove(TEntity entity, TTicketConfig config = default) {
            if (CanRemove(entity, config)) {
                var ticket = CreateRemoveReservation(entity, config);
                removeTickets.Add(ticket);
                OnReserveRemove?.Invoke(this, ticket);
                return ticket;
            } else {
                return default;
            }
        }

        public bool CompleteRemove(TRemoveTicket ticket) {
            if (removeTickets.Contains(ticket)) {
                removeTickets.Remove(ticket);
                Remove(ticket);
                OnRemove?.Invoke(this, ticket);
                return true;
            } else {
                return false;
            }
        }

        public bool CancelRemove(TRemoveTicket ticket) {
            if (removeTickets.Contains(ticket)) {
                removeTickets.Remove(ticket);
                OnCancelRemove?.Invoke(this, ticket);
                return true;
            } else {
                return false;
            }
        }

        public interface IReservationTicket {
            TEntity Entity { get; }
            TTicketConfig Config { get; }
        }
    }

    public abstract class ReservationCollection<TEntity, TTicketConfig> : ReservationCollection<TEntity, ReservationCollection<TEntity, TTicketConfig>.AddTicket, ReservationCollection<TEntity, TTicketConfig>.RemoveTicket, TTicketConfig> {

        protected override AddTicket CreateAddReservation(TEntity entity, TTicketConfig config) {
            return new(entity, config);
        }

        protected override RemoveTicket CreateRemoveReservation(TEntity entity, TTicketConfig config) {
            return new(entity, config);
        }

        public class AddTicket : GenericTicket {
            public AddTicket(TEntity entity, TTicketConfig config) : base(entity, config) {

            }
        }

        public class RemoveTicket : GenericTicket {
            public RemoveTicket(TEntity entity, TTicketConfig config) : base(entity, config) {

            }
        }

        public abstract class GenericTicket : IReservationTicket {
            public TEntity Entity { get; }
            public TTicketConfig Config { get; }

            public GenericTicket(TEntity entity, TTicketConfig config) {
                Entity = entity;
                Config = config;
            }
        }
    }

    public abstract class ReservationCollection<TEntity> : ReservationCollection<TEntity, object> {
        public abstract bool CanAdd(TEntity entity);
        public abstract bool CanRemove(TEntity entity);

        public override bool CanAdd(TEntity entity, object config = null) {
            return CanAdd(entity);
        }

        public override bool CanRemove(TEntity entity, object config = null) {
            return CanRemove(entity);
        }
    }
}
