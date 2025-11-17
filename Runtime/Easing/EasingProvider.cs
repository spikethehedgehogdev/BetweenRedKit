using System;
using System.Collections.Generic;

namespace BetweenRedKit.Easing
{
    /// <summary>
    /// Provides access to easing functions used by <see cref="BetweenRedKit.Core.BetweenProcessor"/>.
    /// </summary>
    /// <remarks>
    /// An easing provider abstracts the mapping between <see cref="EaseType"/> values and
    /// their corresponding mathematical interpolation functions.
    /// </remarks>
    public interface IEasingProvider
    {
        /// <summary>
        /// Retrieves a delegate that computes eased progress for the specified type.
        /// </summary>
        /// <param name="type">The easing curve type to retrieve.</param>
        /// <returns>A function that maps linear progress (0–1) to eased progress.</returns>
        Func<float, float> Get(EaseType type);

        /// <summary>
        /// Registers or replaces a custom easing function for the given type.
        /// </summary>
        /// <param name="type">The easing type to override.</param>
        /// <param name="func">The function that defines easing behavior.</param>
        void Register(EaseType type, Func<float, float> func);
    }

    /// <summary>
    /// Default implementation of <see cref="IEasingProvider"/> containing a set of common easing curves.
    /// </summary>
    /// <remarks>
    /// The provider stores built-in functions in a static array for zero-GC lookup.
    /// Users can register additional or replacement curves at runtime using <see cref="Register"/>.
    /// </remarks>
    public sealed class DefaultEasingProvider : IEasingProvider
    {
        private static readonly Func<float, float>[] BuiltIn = {
            // Linear
            t => t,
            // InQuad
            t => t * t,
            // OutQuad
            t => 1 - (1 - t) * (1 - t),
            // InOutQuad
            t => t < 0.5f ? 2 * t * t : 1 - (float)Math.Pow(-2 * t + 2, 2) / 2,
            // InQuint
            t => t * t * t * t * t,
            // OutQuint
            t => 1 - (float)Math.Pow(1 - t, 5),
            // InOutQuint
            t => t < 0.5f ? 16 * t * t * t * t * t : 1 - (float)Math.Pow(-2 * t + 2, 5) / 2
        };

        private readonly Dictionary<EaseType, Func<float, float>> _custom = new();
        
        /// <inheritdoc />
        public Func<float, float> Get(EaseType type)
        {
            return _custom.TryGetValue(type, out var f)
                ? f
                : BuiltIn[(int)type];
        }

        /// <inheritdoc />
        public void Register(EaseType type, Func<float, float> func)
        {
            _custom[type] = func ?? throw new ArgumentNullException(nameof(func));
        }
    }
}
