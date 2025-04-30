using System;
using System.Collections.Generic;

namespace TarLib.Entities {
    public abstract class QueuableEntityStatesState<TStateTypesEnum> : IQueuableEntityState<TStateTypesEnum>
        where TStateTypesEnum : Enum {

        private int stateIndex = 0;
        private List<IQueuableEntityState<TStateTypesEnum>> states = new();

        public QueuableEntityStatesState(List<IQueuableEntityState<TStateTypesEnum>> states = default) {
            if (states != default) {
                foreach (var state in states) {
                    AddState(state);
                }
            }
        }

        public void AddState(IQueuableEntityState<TStateTypesEnum> state) {
            states.Add(state);
        }

        public abstract bool CanBeCleared { get; }
        public abstract bool CanBeDuplicated { get; }
        public TStateTypesEnum Type => CurrentState != default ? CurrentState.Type : default;
        public IQueuableEntityState<TStateTypesEnum> CurrentState => stateIndex < states.Count && stateIndex >= 0 ? states[stateIndex] : default;

        public void Start() {
            QueueStart();
            CurrentState?.Start();
        }

        public virtual void QueueStart() {

        }

        public EntityStateResponse Update(float appliedTime) {
            var remainingTime = appliedTime;
            do {
                if (CurrentState != default) {
                    bool keepRunning;
                    (keepRunning, remainingTime) = CurrentState.Update(remainingTime);
                    if (!keepRunning) {
                        CurrentState.End();
                        stateIndex += 1;
                        CurrentState?.Start();
                    }
                }
            } while (remainingTime > 0 && CurrentState != default);
            return (CurrentState != default, remainingTime);
        }

        public void End() {
            QueueEnd();
        }

        public virtual void QueueEnd() {

        }
    }
}
