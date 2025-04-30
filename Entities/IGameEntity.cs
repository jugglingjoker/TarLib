using System;

namespace TarLib.Entities {
    public interface IGameEntity<TStateTypesEnum>
        where TStateTypesEnum : Enum {

        IEntityState<TStateTypesEnum> State { get; set; }
        public event EventHandler<(IEntityState<TStateTypesEnum> oldState, IEntityState<TStateTypesEnum> newState)> OnStateChange;
    }
}
