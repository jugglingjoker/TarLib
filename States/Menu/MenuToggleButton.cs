﻿using Microsoft.Xna.Framework;
using System;

namespace TarLib.States {

    public class MenuPlayButton : MenuToggleButton<MenuImage> {

        public event EventHandler OnPlay;
        public event EventHandler OnPause;

        public MenuPlayButton(bool isOn = false, IGameMenu menu = null) : base(isOn, menu) {
            DefaultStyle = new MenuBlockStyle() {
                Default = new MenuBlockStyleRule() {
                    Padding = 4,
                },
                Activate = new MenuBlockStyleRule() {
                    Padding = (6, 4, 2, 4),
                }
            } + base.DefaultStyle;

            OnToggle += MenuPlayButton_OnToggle;
            
        }

        private void MenuPlayButton_OnToggle(object sender, bool isPlaying) {
            if(isPlaying) {
                OnPlay?.Invoke(this, default);
            } else {
                OnPause?.Invoke(this, default);
            }
        }

        protected override MenuImage CreateOffLabel() {
            return new MenuImage("_default_play_button", Menu) {
                Scale = new Vector2(0.5f),
            };
        }

        protected override MenuImage CreateOnLabel() {
            return new MenuImage("_default_pause_button", Menu) {
                Scale = new Vector2(0.5f),
            };
        }
    }

    public abstract class MenuToggleButton<TButtonLabel> : MenuButtonWithStates<bool, TButtonLabel>
        where TButtonLabel : IMenuBlock {

        private bool state;

        public override bool State => state;
        public override TButtonLabel DefaultLabel => OffLabel;

        public TButtonLabel OnLabel;
        public TButtonLabel OffLabel;

        public event EventHandler<bool> OnToggle;

        public MenuToggleButton(bool isOn = false, IGameMenu menu = null) : base(menu) {
            OnLabel = CreateOnLabel();
            OffLabel = CreateOffLabel();

            state = isOn;
            OnLabel.IsVisible = state;
            OffLabel.IsVisible = !state;

            Add(OnLabel);
            Add(OffLabel);

            OnClickEnd += MenuToggleButton_OnClickEnd;
        }

        public bool Toggle() {
            state = !state;
            OnLabel.IsVisible = state;
            OffLabel.IsVisible = !state;
            OnToggle?.Invoke(this, state);
            return state;
        }

        protected abstract TButtonLabel CreateOnLabel();
        protected abstract TButtonLabel CreateOffLabel();

        private void MenuToggleButton_OnClickEnd(object sender, Input.MouseClickEventArgs e) {
            Toggle();
        }
    }
}
