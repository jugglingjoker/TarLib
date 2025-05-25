using Microsoft.Xna.Framework;

namespace TarLib.States {

    public class MenuInputContainerLabel : MenuText {
        public MenuInputContainerLabel(string text, bool canWrapText = false, IGameMenu menu = null) : base(text, canWrapText, menu) {

        }
    }

    public class MenuVector2InputContainer : MenuContainer {
        public MenuInputContainerLabel Label { get; }
        public MenuNumberInputContainer XCoord { get; }
        public MenuNumberInputContainer YCoord { get; }

        protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.CoordinatesInputContainer;

        private int allowedDecimals;
        public int AllowedDecimals {
            get => allowedDecimals;
            set {
                allowedDecimals = value;
                XCoord.Input.AllowedDecimals = value;
                YCoord.Input.AllowedDecimals = value;
            } 
        }

        private bool allowNegatives;
        public bool AllowNegatives {
            get => allowNegatives;
            set {
                allowNegatives = value;
                XCoord.Input.AllowNegatives = value;
                YCoord.Input.AllowNegatives = value;
            }
        }

        public MenuVector2InputContainer(
            Vector2 initialValue,
            bool useInputLabels,
            string label = default,
            IGameMenu menu = null) : base(menu: menu) {

            Label = new MenuInputContainerLabel(
                text: label ?? "",
                canWrapText: false,
                menu: menu);
            Label.OnTextChange += Label_OnTextChange;
            Label.IsVisible = (label != null && label.Length > 0);
            Add(Label);

            XCoord = new MenuNumberInputContainer(
                label: useInputLabels ? "X" : default,
                initialValue: initialValue.X,
                menu: menu);
            Add(XCoord);

            YCoord = new MenuNumberInputContainer(
                label: useInputLabels ? "Y" : default,
                initialValue: initialValue.Y,
                menu: menu);
            Add(YCoord);
        }

        private void Label_OnTextChange(object sender, (string oldValue, string newValue) e) {
            Label.IsVisible = (e.newValue != null && e.newValue.Length > 0);
        }
    }
}
