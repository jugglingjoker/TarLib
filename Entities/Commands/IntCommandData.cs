using System;
using System.Collections.Generic;
using System.Text;

namespace TarLib.Entities.Commands {
    public class IntCommandData : ICommandData {
        public int Data { get; }

        public IntCommandData(int data) {
            Data = data;
        }

        public static implicit operator IntCommandData(int data) => new IntCommandData(data);
        public static implicit operator int(IntCommandData command) => command.Data;
    }
}
