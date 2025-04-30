
namespace TarLib.Entities {
    public struct EntityStateResponse {
        public bool KeepRunning { get; }
        public float TimeRemaining { get; }

        public EntityStateResponse(bool keepRunning, float timeRemaining) {
            KeepRunning = keepRunning;
            TimeRemaining = timeRemaining;
        }

        public static implicit operator EntityStateResponse((bool, float) response) {
            return new EntityStateResponse(response.Item1, response.Item2);
        }

        public static implicit operator EntityStateResponse(bool response) {
            return new EntityStateResponse(response, 0);
        }

        public void Deconstruct(out bool keepRunning, out float timeRemaining) {
            keepRunning = KeepRunning;
            timeRemaining = TimeRemaining;
        }
    }
}
