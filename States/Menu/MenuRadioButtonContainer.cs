using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TarLib.Input;

namespace TarLib.States {

    public class MenuRadioButtonContainer<TValueType> : MenuRadioButtonContainer<TValueType, MenuRadioButton<TValueType>>, IEquatable<TValueType>
        where TValueType : IEquatable<TValueType> {
        
        public MenuRadioButtonContainer(IGameMenu menu = null, Dictionary<TValueType, string> values = null, TValueType defaultValue = default) : base(menu, values, defaultValue) {
        
        }

        public override bool Equals(TValueType other) {
            return SelectedValue?.Equals(other) ?? false;
        }
    }

    public abstract class MenuRadioButtonContainer<TValueType, TRadioButton> : MenuContainer<TRadioButton>
        where TRadioButton : IMenuRadioButton<TValueType>, new() {

        public MenuRadioButtonContainer(
            IGameMenu menu = null,
            Dictionary<TValueType, string> values = default,
            TValueType defaultValue = default) : base(menu: menu) {
            SetValues(values, defaultValue);
            selectedButton.OnChange += SelectedButton_OnChange;

            DefaultStyle = new MenuBlockStyleRule() {
                WidthStyle = SizeStyle.FitParent,
            };
        }

        protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.RadioButtonContainer;

        private readonly ObservableVariable<TRadioButton> selectedButton = new();
        public TRadioButton SelectedButton {
            get => selectedButton.Value;
            private set => selectedButton.Value = value;
        }

        public TValueType SelectedValue {
            get => SelectedButton != null ? SelectedButton.Value : default;
            set {
                blocks.ForEach(menuBlock => menuBlock.IsSelected = false);
                foreach (var menuBlock in MenuBlocks) {
                    if (menuBlock.Value.Equals(value)) {
                        menuBlock.IsSelected = true;
                        SelectedButton = menuBlock;
                        return;
                    }
                }
                // TODO: Maybe keep value as is?
                SelectedButton = default;
            }
        }

        public event EventHandler<(TValueType oldValue, TValueType newValue)> OnChange;

        private void SelectedButton_OnChange(object sender, (TRadioButton oldValue, TRadioButton newValue) e) {
            OnChange?.Invoke(this, (e.oldValue != null ? e.oldValue.Value : default, e.newValue != null ? e.newValue.Value : default));
        }

        public void SetValues(Dictionary<TValueType, string> values, TValueType value) {
            Clear();
            if (values != null) {
                foreach (var valuePair in values) {
                    var radioButton = new TRadioButton {
                        Text = valuePair.Value,
                        Value = valuePair.Key,
                        Parent = this
                    };

                    radioButton.OnClickStart += (object sender, MouseClickEventArgs e) => {
                        if (e.MouseButton == MouseButton.LeftButton) {
                            e.FlagAsUsed();
                        }
                    };

                    radioButton.OnClickEnd += (object sender, MouseClickEventArgs e) => {
                        if(e.MouseButton == MouseButton.LeftButton) {
                            SelectedValue = radioButton.Value;
                            e.FlagAsUsed();
                        }
                    };
                    Add(radioButton);
                    if(valuePair.Key.Equals(value)) {
                        radioButton.IsSelected = true;
                        selectedButton.SetValueNoChange(radioButton);
                    }
                }
            }
        }

        public abstract bool Equals(TValueType value);
    }
}
