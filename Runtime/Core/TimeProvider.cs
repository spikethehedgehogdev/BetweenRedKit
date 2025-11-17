using UnityEngine;

namespace BetweenRedKit.Core
{
    /// <summary>
    /// Provides delta-time values for updating tweens.
    /// </summary>
    /// <remarks>
    /// Time providers abstract away frame-dependent timing, allowing
    /// deterministic updates or external control (e.g. manual stepping in tests).
    /// </remarks>
    public interface ITimeProvider
    {
        /// <summary>
        /// The delta time value (in seconds) used to advance active tweens.
        /// </summary>
        float DeltaTime { get; }
    }

    /// <summary>
    /// Default Unity-based time provider using <see cref="Time.deltaTime"/>.
    /// </summary>
    /// <remarks>
    /// Used by default in <see cref="BetweenProcessor"/> for real-time updates.
    /// </remarks>
    public sealed class UnityTimeProvider : ITimeProvider
    {
        /// <inheritdoc />
        public float DeltaTime => Time.deltaTime;
    }

    /// <summary>
    /// Manual time provider for deterministic or controlled simulation.
    /// </summary>
    /// <remarks>
    /// Useful for unit testing or manual stepping outside Unity’s frame loop.
    /// The <see cref="DeltaTime"/> value can be set explicitly each update cycle.
    /// </remarks>
    public sealed class ManualTimeProvider : ITimeProvider
    {
        /// <inheritdoc />
        public float DeltaTime { get; set; }
    }
}