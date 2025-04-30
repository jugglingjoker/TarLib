using System;
using System.Collections.Generic;
using TarLib.Encoding;

namespace TarLib.Entities {

    public class EntityStateManager<TEntity, TStateTypesEnum> : IEncodable
        where TEntity : IGameEntity<TStateTypesEnum>
        where TStateTypesEnum : Enum {
        private TEntity Entity { get; }

        private IQueuableEntityState<TStateTypesEnum> DefaultState;
        private bool IsCurrentlyUpdating;
        private bool PendingClearRequest;
        private Queue<IQueuableEntityState<TStateTypesEnum>> Queue { get; } = new Queue<IQueuableEntityState<TStateTypesEnum>>();
        private Queue<IQueuableEntityState<TStateTypesEnum>> ToQueue { get; } = new Queue<IQueuableEntityState<TStateTypesEnum>>();

        public IQueuableEntityState<TStateTypesEnum> Active => Queue.Count > 0 ? Queue.Peek() : DefaultState;

        public EntityStateManager(TEntity entity, IQueuableEntityState<TStateTypesEnum> defaultState) {
            Entity = entity;
            DefaultState = defaultState;
            Entity.State = DefaultState;
            Queue = new Queue<IQueuableEntityState<TStateTypesEnum>>();
        }

        public event EventHandler<TEntity> OnClear;
        public void Clear() {
            if (IsCurrentlyUpdating) {
                PendingClearRequest = true;
                if(ToQueue.Count > 0) {
                    var backupQueue = new List<IQueuableEntityState<TStateTypesEnum>>();
                    while(ToQueue.Count > 0) {
                        var backupState = ToQueue.Dequeue();
                        if(!backupState.CanBeCleared) {
                            backupQueue.Add(backupState);
                        }
                    }
                    foreach(var backupState in backupQueue) {
                        ToQueue.Enqueue(backupState);
                    }
                }
            } else {
                var actionsToKeep = new Queue<IQueuableEntityState<TStateTypesEnum>>();
                while (Queue.Count > 0) {
                    var action = Queue.Dequeue();
                    if (!action.CanBeCleared) {
                        actionsToKeep.Enqueue(action);
                    } else {
                        action.End();
                    }
                }
                while (actionsToKeep.Count > 0) {
                    Add(actionsToKeep.Dequeue());
                }
                OnClear?.Invoke(this, Entity);
            }
        }

        public void Update(float elapsedTime) {
            var remainingTime = elapsedTime;
            bool keepRunning;

            do {
                IsCurrentlyUpdating = true;
                (keepRunning, remainingTime) = Entity.State.Update(remainingTime);
                IsCurrentlyUpdating = false;

                if (!keepRunning && Queue.Count > 0) {
                    Queue.Dequeue();
                }
                if (PendingClearRequest) {
                    PendingClearRequest = false;
                    Clear();
                    while (ToQueue.Count > 0) {
                        Add(ToQueue.Dequeue());
                    }
                }
                Entity.State = Active;
            } while (remainingTime > 0 && Queue.Count > 0);
        }

        public void Add(IQueuableEntityState<TStateTypesEnum> action) {
            if (PendingClearRequest) {
                this.ToQueue.Enqueue(action);
            } else {
                Queue.Enqueue(action);
                Entity.State = Active;
            }
        }

        public static EntityStateManager<TEntity, TStateTypesEnum> operator +(EntityStateManager<TEntity, TStateTypesEnum> queue, IQueuableEntityState<TStateTypesEnum> action) {
            queue.Add(action);
            return queue;
        }
    }
}
