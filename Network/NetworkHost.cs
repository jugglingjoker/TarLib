using System;

using TarLib.Encoding;

namespace TarLib.Networking {
    public class NetworkHost {

        public NetworkHost() {

        }

        public event EventHandler<INetworkMessage> OnMessageReceived;
        public void MessageReceived(NetworkHostMessage message) {
            var isSystem = false;
            if (isSystem) {

            } else {
                OnMessageReceived?.Invoke(this, message);
            }
        }

        public void SendMessage(NetworkDestination destination, INetworkMessage message) {

        }
    }

    public struct NetworkDestination {
        public string Address { get; set; }
    }

    public struct NetworkHostMessage : INetworkMessage {
        public static class SystemMessageType {
            public const int PING = 0;
            public const int PING_RESPONSE = 1;
            public const int CONNECT_REQUEST = 2;
            public const int DISCONNECT = 3;
        }

        public int Type { get; }
        public IEncodable Message { get; }
    }

    public struct NetworkClientMessage : INetworkMessage {
        public static class SystemMessageType {
            public const int PING = 0;
            public const int PING_RESPONSE = 1;
            public const int CONNECT_DENY = 2;
            public const int CONNECT_ACCEPT = 3;
            public const int KICK = 4;
        }

        public int Type { get; }
        public IEncodable Message { get; }
    }

    public interface INetworkMessage {
        public int Type { get; }
        public IEncodable Message { get; }
    }
}
