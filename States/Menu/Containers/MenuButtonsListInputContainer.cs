using TarLib.States;

namespace StickyFeet.States {
    public class MenuButtonsListInputContainer<TValue> : MenuInputContainer<MenuButtonsList<TValue>> {
        public MenuButtonsListInputContainer(string label, TValue[] values, TValue selectedValues = default, IGameMenu menu = null) : this(label, values, new[] { selectedValues }, menu) {

        }

        public MenuButtonsListInputContainer(string label, TValue[] values, TValue[] selectedValues = default, IGameMenu menu = null) : base(label, new MenuButtonsList<TValue>(menu: menu, values: values, selectedValues: selectedValues), menu) {

        }
    }
}
