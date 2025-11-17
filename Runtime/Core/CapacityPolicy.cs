using System;

namespace BetweenRedKit.Core
{
    /// <summary>
    /// Defines a strategy for expanding the tween slot pool
    /// when the <see cref="BetweenProcessor"/> runs out of capacity.
    /// </summary>
    /// <remarks>
    /// Custom implementations can control memory growth behavior.
    /// For example, embedded or memory-constrained systems may prefer
    /// a fixed-capacity policy to prevent dynamic allocations.
    /// </remarks>
    public interface ICapacityPolicy
    {
        /// <summary>
        /// Calculates the next pool size based on the current capacity.
        /// </summary>
        /// <param name="currentCapacity">The current pool size.</param>
        /// <returns>The new capacity after growth.</returns>
        int Grow(int currentCapacity);
    }

    /// <summary>
    /// Expands the pool by doubling its size each time, up to a maximum limit.
    /// </summary>
    /// <remarks>
    /// Default policy used by <see cref="BetweenProcessor"/>.
    /// Provides amortized O(1) allocation behavior and predictable scaling.
    /// </remarks>
    public sealed class DoublingCapacityPolicy : ICapacityPolicy
    {
        private readonly int _max;

        /// <summary>
        /// Initializes a new doubling policy with an optional maximum capacity limit.
        /// </summary>
        /// <param name="max">The maximum allowed capacity (default: 1,048,576).</param>
        public DoublingCapacityPolicy(int max = 1 << 20) => _max = max;

        /// <inheritdoc />
        public int Grow(int current) => Math.Min(_max, Math.Max(4, current * 2));
    }

    /// <summary>
    /// Disables automatic pool growth, keeping capacity constant.
    /// </summary>
    /// <remarks>
    /// Suitable for deterministic or embedded environments where
    /// dynamic allocations are not allowed.
    /// </remarks>
    public sealed class FixedCapacityPolicy : ICapacityPolicy
    {
        /// <inheritdoc />
        public int Grow(int current) => current;
    }
}