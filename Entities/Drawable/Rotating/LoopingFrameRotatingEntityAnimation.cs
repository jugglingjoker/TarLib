using System;
using System.Collections.Generic;
using System.Linq;

namespace TarLib.Entities.Drawable {
    public class LoopingFrameRotatingEntityAnimation<TEntity> : RotatingEntityAnimation<TEntity>
        where TEntity : IDrawableEntityWithRotatingAnimation {

        private bool RandomizedStart;
        private float RunningTime;
        private readonly TimeSpan FrameLength;
        private int CurrentFrame;
        private List<int> Frames { get; }

        public override int FrameY => Frames[CurrentFrame];

        public LoopingFrameRotatingEntityAnimation(TEntity entity, TimeSpan frameLength, int[] frames, bool randomizedStart = false) : base(entity) {
            FrameLength = frameLength;
            Frames = frames.Length > 0 ? frames.ToList() : new List<int>() { 0 };
            RandomizedStart = randomizedStart;
        }

        public override void Update(float elapsedTime) {
            RunningTime += elapsedTime;
            CurrentFrame = (int)(RunningTime / FrameLength.TotalSeconds) % Frames.Count;
        }

        public override void Reset() {
            RunningTime = RandomizedStart ? (float)(FrameLength.TotalSeconds * Frames.Count * RandomMod) : 0;
        }
    }
}
