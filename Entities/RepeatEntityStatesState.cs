using System;
using System.Collections.Generic;

namespace TarLib.Entities {

    public abstract class RepeatEntityStatesState<TStateTypesEnum> : IQueuableEntityState<TStateTypesEnum>
        where TStateTypesEnum : Enum {

        private int stateIndex = 0;
        private List<IQueuableEntityState<TStateTypesEnum>> states = new();

        public RepeatEntityStatesState(List<IQueuableEntityState<TStateTypesEnum>> states = default) {
            if(states != default) {
                foreach(var state in states) {
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
            LoopStart();
            CurrentState?.Start();
        }

        public virtual void LoopStart() {
            
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
                        if (stateIndex >= states.Count) {
                            LoopEnd();
                            if (CanContinueRepeating) {
                                stateIndex = 0;
                                LoopStart();
                                CurrentState.Start();
                            }
                        } else {
                            CurrentState.Start();
                        }
                    }
                }
            } while (remainingTime > 0 && CurrentState != default);
            return (CurrentState != default, remainingTime);
        }

        public void End() {
            LoopEnd();
        }

        public virtual void LoopEnd() {

        }

        public abstract bool CanContinueRepeating { get; }
    }
}
