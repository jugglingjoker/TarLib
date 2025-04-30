using System;
using System.Collections.Generic;
using System.Linq;

namespace TarLib {


    public class TwoDimGrid<TValue> : TwoDimDictionary<int, TValue> {

        public override TValue this[int x, int y]  { 
            get => base[x, y];
            set {
                base[x, y] = value;
                RecalculateExtremes();
            }
        }

        public int MinX { get; protected set; }
        public int MaxX { get; protected set; }
        public int MinY { get; protected set; }
        public int MaxY { get; protected set; }

        private void RecalculateExtremes() {
            MinX = data.Keys.Min();
            MaxX = data.Keys.Max();
            MinY = data.Select(dataPair => dataPair.Value.Keys.Min()).Min();
            MaxY = data.Select(dataPair => dataPair.Value.Keys.Max()).Max();
        }
    }

    public class TwoDimDictionary<TKey, TValue> {
        protected SortedDictionary<TKey, Dictionary<TKey, TValue>> data = new();

        public virtual TValue this[TKey x, TKey y] {
            get => data.ContainsKey(x) && data[x].ContainsKey(y) ? data[x][y] : default;
            set {
                if (!data.ContainsKey(x)) {
                    data[x] = new Dictionary<TKey, TValue>();
                }
                data[x][y] = value;
            }
        }

        public void Clear() {
            foreach(var keyPair in data) {
                keyPair.Value.Clear();
            }
            data.Clear();
        }

        public void Remove(TKey x, TKey y) {
            if(data.ContainsKey(x)) {
                data[x].Remove(y);
                if(data[x].Count == 0) {
                    data.Remove(x);
                }
            }
        }
    }
}
