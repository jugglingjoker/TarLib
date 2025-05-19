using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TarLib.Input;

namespace TarLib.States {

    public class MenuButtonsList<TValue> : MenuButtonsList<TValue, MenuButtonsList<TValue>.Button> {
        public MenuButtonsList(IGameMenu menu = null, TValue[] values = default, TValue[] selectedValues = default, uint minValues = 1, uint maxValues = 1) : base(menu, values, selectedValues, minValues, maxValues) {

        }

        protected override Button CreateButton(string label, TValue value, IGameMenu menu = default) {
            return new(label, value);
        }

        public class Button : MenuTextButton {
            protected override MenuBlockStyleTypeList StyleTypes => base.StyleTypes + MenuBlockStyleType.ButtonsListButton;
            public TValue Value { get; }

            public Button(string label, TValue value, IGameMenu menu = null) : base(label, false, menu) {
                Value = value;

                DefaultStyle = new MenuBlockStyle() {
                    Default = new MenuBlockStyleRule() {
                        Margin = (0, 0, 0, 1),
                        Padding = (4, 6, -2, 6),
                        BackgroundColor = Color.LightGray,
                        BorderColor = Color.DarkGray,
                    },
                    Select = new MenuBlockStyleRule() {
                        BackgroundColor = base.DefaultStyle.Default.BackgroundColor,
                        BorderColor = base.DefaultStyle.Default.BorderColor
                    },
                } + base.DefaultStyle;
            }
        }
    }

    public abstract class MenuButtonsList<TValue, TMenuButton> : MenuContainer<TMenuButton>
        where TMenuButton : MenuButton {

        private ObservableDictionary<TMenuButton, TValue> values = new();
        private ObservableList<TMenuButton> selectedButtons = new();

        protected override MenuBlockStyleTypeList StyleTypes => MenuBlockStyleType.ButtonsList;

        public uint MinValues { get; }
        public uint MaxValues { get; }

        public IReadOnlyList<TValue> SelectedValues => selectedButtons.Where(button => values.ContainsKey(button)).Select(button => values[button]).ToList();

        public event EventHandler OnSelectionChange;

        public MenuButtonsList(IGameMenu menu = null, TValue[] values = default, TValue[] selectedValues = default, uint minValues = 1, uint maxValues = 1) : base(menu) {
            DefaultStyle = new MenuBlockStyleRule() {
                ContentDirection = ContentDirectionStyle.Row,
                ContentVerticalAlign = AlignStyle.Start,
                ContentHorizontalAlign = AlignStyle.Start,
                Margin = (2, 4),
            };

            MinValues = Math.Max(minValues, 1);
            MaxValues = Math.Max(maxValues, 1);

            if(values != default) {
                foreach (var value in values) {
                    Add(label: value.ToString(), value: value);
                }
            }
            if(selectedValues != default) {
                foreach (var value in selectedValues) {
                    Toggle(value);
                }
            }
        }

        public override void Add(IMenuBlock menuBlock) {
            base.Add(menuBlock);
            if (menuBlock is TMenuButton button) {
                button.OnClickEnd += Button_OnClickEnd;
            }
        }

        public virtual void Add(string label, TValue value) {
            var button = CreateButton(label: label, value: value);
            values.Add(button, value);
            Add(button);
        }

        public void Toggle(TValue value) {
            values.Where(kvp => kvp.Value.Equals(value)).ToList().ForEach(kvp => Toggle(kvp.Key));
        }

        public void Toggle(TMenuButton button) {
            if(selectedButtons.Contains(button)) {
                // if this button is already selected 
                if (selectedButtons.Count > MinValues) {
                    // and removing it would not take the selection below the min, remove it
                    selectedButtons.Remove(button);
                    button.IsSelected = false;
                    OnSelectionChange?.Invoke(this, default);
                }
            } else {
                // if this button is not selected
                if (selectedButtons.Count < MaxValues) {
                    // and if there is room in the selection for it, add it
                    selectedButtons.Add(button);
                    button.IsSelected = true;
                    OnSelectionChange?.Invoke(this, default);
                } else {
                    // or if there is no room, remove the first added
                    var firstButton = selectedButtons[0];
                    selectedButtons.RemoveAt(0);
                    firstButton.IsSelected = false;

                    selectedButtons.Add(button);
                    button.IsSelected = true;
                    OnSelectionChange?.Invoke(this, default);
                }
            }
        }

        protected abstract TMenuButton CreateButton(string label, TValue value, IGameMenu menu = default);

        private void Button_OnClickEnd(object sender, MouseClickEventArgs e) {
            if(sender is TMenuButton button) {
                Toggle(button);
            }
        }
    }
}
