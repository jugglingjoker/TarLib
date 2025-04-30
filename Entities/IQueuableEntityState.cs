using System;

namespace TarLib.Entities {

    public interface IQueuableEntityState<TStateTypesEnum> : IEntityState<TStateTypesEnum>
        where TStateTypesEnum : Enum {
        bool CanBeCleared { get; }
        bool CanBeDuplicated { get; }
    }
}
