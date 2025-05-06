using System;

namespace TarLib.Entities {

    public abstract class SimpleGameEntity<TCommandTypesEnum, TStateTypesEnum> : ICommandableGameEntity<TCommandTypesEnum, TStateTypesEnum>, IGameEntity<TStateTypesEnum>
        where TCommandTypesEnum : Enum
        where TStateTypesEnum : Enum {

        protected EntityStateManager<IGameEntity<TStateTypesEnum>, TStateTypesEnum> StateManager { get; set; }

        public event EventHandler<(IEntityState<TStateTypesEnum> oldState, IEntityState<TStateTypesEnum> newState)> OnStateChange;

        private ObservableVariable<IEntityState<TStateTypesEnum>> state = new();
        public IEntityState<TStateTypesEnum> State {
            get => state.Value;
            set => state.Value = value;
        }

        public abstract IQueuableEntityState<TStateTypesEnum> DefaultState { get; }
        public abstract void ExecuteCommand(TCommandTypesEnum command, ICommandData commandData);

        public SimpleGameEntity() {
            state.OnChange += State_OnChange;
            StateManager = new EntityStateManager<IGameEntity<TStateTypesEnum>, TStateTypesEnum>(this, DefaultState);
        }

        public virtual void Update(float elapsedTime) {
            StateManager.Update(elapsedTime);
        }

        private void State_OnChange(object sender, (IEntityState<TStateTypesEnum> oldValue, IEntityState<TStateTypesEnum> newValue) e) {
            e.oldValue?.End();
            e.newValue?.Start();
            OnStateChange?.Invoke(this, e);
        }
    }
}
