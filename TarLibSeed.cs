
namespace TarLib {
    public struct TarLibSeed {
        public int SeedValue { get; }

        public TarLibSeed(int seed) {
            SeedValue = seed;
        }

        public static implicit operator TarLibSeed(int seed) {
            return new TarLibSeed(seed);
        }

        public static implicit operator int(TarLibSeed seed) {
            return seed.SeedValue;
        }
    }
}
