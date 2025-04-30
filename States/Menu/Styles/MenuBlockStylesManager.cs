using System;
using System.Collections.Generic;
using System.Linq;

namespace TarLib.States {
    public class MenuBlockStylesManager {
        public TarGame Game { get; }

        private Dictionary<MenuBlockStyleType, MenuBlockStyle> stylesByType = new Dictionary<MenuBlockStyleType, MenuBlockStyle>();
        private Dictionary<string, MenuBlockStyle> stylesByTag = new Dictionary<string, MenuBlockStyle>();

        public MenuBlockStylesManager(TarGame game) {
            Game = game;
        }

        public void Add(string tag, MenuBlockStyle menuStyle) {
            stylesByTag[tag] = menuStyle;
        }

        public void Add(MenuBlockStyleType type, MenuBlockStyle menuStyle) {
            stylesByType[type] = menuStyle;
        }

        public MenuBlockStyle Get(MenuBlockStyleTypeList types, MenuBlockStyleTagList tags) {
            MenuBlockStyle style = default;
            foreach (var tag in tags.Reverse().Where(tag => stylesByTag.ContainsKey(tag))) {
                style += stylesByTag[tag];
            }
            foreach (var type in types.Reverse().Where(type => stylesByType.ContainsKey(type))) {
                style += stylesByType[type];
            }
            return style;
        }
    }
}
