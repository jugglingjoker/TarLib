using System;
using System.Collections.Generic;
using System.Linq;

namespace TarLib.Extensions {
    public static class ListExtensions {

        private static Random defaultRandom;
        private static Dictionary<TarLibSeed, Random> randoms = new();

        public static T Random<T>(this IEnumerable<T> list, TarLibSeed? seed = default) {
            if(list.Count() == 0) {
                // TODO: throw exception?
                return default;
            } else if(list.Count() == 1) {
                return list.First();
            } else {
                Random random;
                if (seed != default) {
                    if(!randoms.ContainsKey(seed.Value)) {
                        randoms[seed.Value] = new Random(seed.Value);
                    }
                    random = randoms[seed.Value];
                } else {
                    if(defaultRandom == default) {
                        defaultRandom = new Random();
                    }
                    random = defaultRandom;
                }
                return list.ElementAt(random.Next(0, list.Count()));
            }
        }

        public static IEnumerable<T> RandomSet<T>(this IEnumerable<T> list, int count, TarLibSeed? seed = default) {
            if(count >= list.Count()) {
                return list.ToList();
            } else {
                var shuffled = list.ToList();
                shuffled.Shuffle(seed);
                return shuffled.GetRange(0, Math.Min(count, shuffled.Count()));
            }
        }

        public static void Shuffle<T>(this IList<T> list, TarLibSeed? seed = default) {
            Random random;
            if (seed != default) {
                if (!randoms.ContainsKey(seed.Value)) {
                    randoms[seed.Value] = new Random(seed.Value);
                }
                random = randoms[seed.Value];
            } else {
                if (defaultRandom == default) {
                    defaultRandom = new Random();
                }
                random = defaultRandom;
            }

            int n = list.Count;
            while (n > 1) {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
