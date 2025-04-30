using System;

namespace TarLib.Entities {
    public interface ICommandableGameEntity<TCommandTypesEnum, TStateTypesEnum> : IGameEntity<TStateTypesEnum>
        where TCommandTypesEnum : Enum
        where TStateTypesEnum : Enum {

        void ExecuteCommand(TCommandTypesEnum command, ICommandData data);
    }
}
