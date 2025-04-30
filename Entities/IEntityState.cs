using System;
using TarLib.Encoding;

namespace TarLib.Entities {

    public interface IEntityState<TStateTypesEnum> : IEncodable
        where TStateTypesEnum : Enum {
        TStateTypesEnum Type { get; }
        void Start();
        EntityStateResponse Update(float appliedTime);
        void End();
    }
}
