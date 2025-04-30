using System;
using TarLib.States;

namespace TarLib {

    public class ObservableVariable<TVariableType> {
        private TVariableType storedValue;
        public TVariableType Value {
            get => storedValue;
            set {
                if (!(storedValue?.Equals(value) ?? value == null)) {
                    var oldValue = storedValue;
                    storedValue = value;
                    OnChange?.Invoke(this, (oldValue, value));
                }
            }
        }

        public event EventHandler<(TVariableType oldValue, TVariableType newValue)> OnChange;

        public ObservableVariable(TVariableType value = default) {
            Value = value;
        }

        public void SetValueNoChange(TVariableType value) {
            storedValue = value;
        }

        public static implicit operator TVariableType(ObservableVariable<TVariableType> observableVariable) {
            return observableVariable.Value;
        }
    }
}
