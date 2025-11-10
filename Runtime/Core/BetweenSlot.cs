using System;

namespace BetweenRedKit.Core
{
    internal struct BetweenSlot
    {
        public bool Active;
        public int Version;
        public IBetweenTarget Target;
        public float Time, Duration;
        public Func<float, float> Ease;
        public Action OnComplete;
        public bool Paused;
    }
}