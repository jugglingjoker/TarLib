using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TarLib.Input;

namespace TarLib.States {

    public interface IMenuRadioButton<TValueType> : IMenuContainer {
        string Text { get; set; }
        TValueType Value { get; set; }
    }

    public class MenuRadioButton<TValueType> : MenuContainer, IMenuRadioButton<TValueType> {

        public MenuRadioButton() : base(default) {
            Indicator = new IndicatorImage(this);
            Label = new InputLabel(this, "");

            Add(Indicator);
            Add(Label);

            DefaultStyle = new MenuBlockStyle() {
                Default = new MenuBlockStyleRule() {
                    WidthStyle = SizeStyle.FitParent,
                    Padding = 5,
                },
                Select = new MenuBlockStyleRule() {
                    BackgroundColor = Color.Gray
                }
            } + DefaultStyle;
        }

        public IndicatorImage Indicator { get; }
        public InputLabel Label { get; }
  
        public string Text {
            get => Label.Source.Text;
            set => Label.Source.Text = value;
        }
        public TValueType Value { get; set; }

        protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.RadioButton;

        public class IndicatorImage : MenuImage {
            public MenuRadioButton<TValueType> RadioButton { get; }

            protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.RadioButtonIndicator;

            public IndicatorImage(MenuRadioButton<TValueType> radioButton, IGameMenu menu = null) : base("radioButton", menu) {
                RadioButton = radioButton;
            }
        }

        public class InputLabel : MenuText {
            public MenuRadioButton<TValueType> RadioButton { get; }

            protected override MenuBlockStyleTypeList StyleTypes => base.StyleTypes + MenuBlockStyleType.RadioButtonLabel;

            public InputLabel(MenuRadioButton<TValueType> radioButton, string text, IGameMenu menu = null) : base(text, false, menu) {
                RadioButton = radioButton;
            }
        }
    }
}
