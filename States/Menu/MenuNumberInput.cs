using System;

namespace TarLib.States {
    public class MenuNumberInput : MenuInput<float?> {
        public int AllowedDecimals { get; set; } = 0;
        public bool AllowNegatives { get; set; } = true;
        public float MinValue { get; set; } = float.MinValue;
        public float MaxValue { get; set; } = float.MaxValue;

        protected override MenuBlockStyleTypeList StyleTypes => base.StyleTypes + MenuBlockStyleType.NumberInput;

        public MenuNumberInput(float initialValue, IGameMenu menu = null) : base(initialValue, menu) {
            DefaultValue = 0;
        }

        protected override bool IsValidTemporaryString(string testInput) {
            return testInput == "-";
        }

        protected override float? TranslateInput(string testInput) {
            try {
                var result = float.Parse(testInput);
                if((result >= 0 || AllowNegatives) && Math.Round(result, AllowedDecimals) == result && result >= MinValue && result <= MaxValue) {
                    return result;
                }
            } catch {

            }
            return null;
        }
    }
}
