using System;
using TarLib.States;

namespace TarLib {

    public class ObservableVariable<T> {
        private T storedValue;
        public T Value {
            get => storedValue;
            set {
                if (!(storedValue?.Equals(value) ?? value == null)) {
                    var oldValue = storedValue;
                    storedValue = value;
                    OnChange?.Invoke(this, (oldValue, value));
                }
            }
        }

        public event EventHandler<(T oldValue, T newValue)> OnChange;

        public ObservableVariable(T value = default) {
            Value = value;
        }

        public void SetValueNoChange(T value) {
            storedValue = value;
        }

        public static implicit operator T(ObservableVariable<T> observableVariable) {
            return observableVariable.Value;
        }
    }
}
