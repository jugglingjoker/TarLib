using System;
using System.Collections.Generic;
using System.Text;

namespace TarLib.Entities.Commands {
    public class FloatCommandData : ICommandData {
        public float Data { get; }

        public FloatCommandData(float data) {
            Data = data;
        }

        public static implicit operator FloatCommandData(float data) => new FloatCommandData(data);
        public static implicit operator float(FloatCommandData command) => command.Data;
    }
}
