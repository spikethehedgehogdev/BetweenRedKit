using System;

namespace BetweenRedKit.Core
{
    /// <summary>
    /// Internal data container representing a single active tween slot
    /// within the <see cref="BetweenProcessor"/> pool.
    /// </summary>
    /// <remarks>
    /// Slots are stored in a densely packed array for O(1) insertion and removal.
    /// This struct is never exposed publicly to maintain zero-GC safety and prevent misuse.
    /// </remarks>
    internal struct BetweenSlot
    {
        /// <summary>
        /// Indicates whether the slot is currently active.
        /// </summary>
        public bool Active;

        /// <summary>
        /// Version counter used for handle validation and recycling protection.
        /// </summary>
        public int Version;

        /// <summary>
        /// The tween target to update.
        /// </summary>
        public IBetweenTarget Target;

        /// <summary>
        /// Current elapsed time and total duration, in seconds.
        /// </summary>
        public float Time, Duration;

        /// <summary>
        /// Easing function delegate used to transform normalized progress.
        /// </summary>
        public Func<float, float> Ease;

        /// <summary>
        /// Callback invoked once when the tween finishes.
        /// </summary>
        public Action OnComplete;

        /// <summary>
        /// Whether this tween is currently paused.
        /// </summary>
        public bool Paused;
    }
}