using System;
using TarLib.Encoding;

namespace TarLib.Entities {
    public interface ITickBasedEntityState<TStateTypesEnum> : IEncodable
        where TStateTypesEnum : Enum {

        TStateTypesEnum Type { get; }
        void Start();
        EntityStateResponse UpdateTick(float appliedTime);
        void End();
    }
}
