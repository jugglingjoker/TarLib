using System;
using System.Collections.Generic;

namespace TarLib.States {
    public class MenuRadioButtonContainerEnum<TValueType> : MenuRadioButtonContainer<TValueType, MenuRadioButton<TValueType>>
        where TValueType : Enum {

        public MenuRadioButtonContainerEnum(IGameMenu menu = null, Dictionary<TValueType, string> values = null, TValueType defaultValue = default) : base(menu, values, defaultValue) {

        }

        public override bool Equals(TValueType value) {
            return SelectedValue?.Equals(value) ?? false;
        }
    }
}
