using System;
using System.Collections.Generic;
using System.Text;

namespace TarLib.Entities.Commands {
    public class BoolCommandData : ICommandData {
        public bool Data { get; }

        public BoolCommandData(bool data) {
            Data = data;
        }

        public static implicit operator BoolCommandData(bool data) => new BoolCommandData(data);
        public static implicit operator bool(BoolCommandData command) => command.Data;
    }
}
