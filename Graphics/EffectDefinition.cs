using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TarLib.Graphics {
    public class EffectDefinition {
        public Effect Effect {
            get {
                var effect = BaseGame.Effects[Label];
                foreach(var modifier in Modifiers) {
                    modifier.ApplyTo(effect);
                }
                return effect;
            }
        }

        public TarGame BaseGame { get; }
        public string Label { get; }
        public List<EffectModifier> Modifiers { get; }

        public EffectDefinition(TarGame baseGame, string label, List<EffectModifier> modifiers = default) {
            BaseGame = baseGame;
            Label = label;
            Modifiers = new List<EffectModifier>();
            if(modifiers != default) {
                foreach(var modifier in modifiers) {
                    Modifiers.Add(modifier);
                }
            }
        }
    }

    public abstract class EffectModifier {
        public string Label { get; }

        public EffectModifier(string label) {
            Label = label;
        }

        public abstract void ApplyTo(Effect effect);
    }

    public abstract class SimpleEffectModifier<TVariable> : EffectModifier {
        public TVariable Value { get; set; }

        public SimpleEffectModifier(string label, TVariable value) : base(label) {
            Value = value;
        }
    }

    public class ColorEffectModifier : SimpleEffectModifier<Color> {
        public ColorEffectModifier(string label, Color value) : base(label, value) {
            
        }

        public override void ApplyTo(Effect effect) {
            effect.Parameters[Label].SetValue(Value.ToVector4());
        }
    }

    public class Vector2EffectModifier : SimpleEffectModifier<Vector2> {
        public Vector2EffectModifier(string label, Vector2 value) : base(label, value) {
            
        }

        public override void ApplyTo(Effect effect) {
            effect.Parameters[Label].SetValue(Value);
        }
    }

    public class FloatEffectModifier : SimpleEffectModifier<float> {
        public FloatEffectModifier(string label, float value) : base(label, value) {
            
        }

        public override void ApplyTo(Effect effect) {
            effect.Parameters[Label].SetValue(Value);
        }
    }
}
