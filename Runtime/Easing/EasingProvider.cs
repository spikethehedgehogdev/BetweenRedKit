using System;
using System.Collections.Generic;
using UnityEngine;

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
        private static readonly Func<float, float>[] BuiltIn =
        {
            // Linear
            t => t, 
            
            // InSine
            t => 1f - Mathf.Cos((t * Mathf.PI) / 2f),
            // OutSine
            t => Mathf.Sin((t * Mathf.PI) / 2f),
            // InOutSine
            t => -(Mathf.Cos(Mathf.PI * t) - 1f) / 2f,
            
            // InQuad
            t => t * t,
            // OutQuad
            t => 1f - (1f - t) * (1f - t),
            // InOutQuad
            t => t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f,
            
            // InCubic
            t => t * t * t,
            // OutCubic
            t =>
            {
                t -= 1f;
                return t * t * t + 1f;
            },
            // InOutCubic
            t => t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f,
            
            // InQuart
            t => t * t * t * t,
            // OutQuart
            t =>
            {
                t -= 1f;
                return 1f - t * t * t * t;
            },
            // InOutQuart
            t => t < 0.5f ? 8f * t * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 4f) / 2f,
            
            // InQuint
            t => t * t * t * t * t,
            // OutQuint
            t => 1f - Mathf.Pow(1f - t, 5f),
            // InOutQuint
            t => t < 0.5f ? 16f * t * t * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 5f) / 2f,
            
            // InExpo
            t => t == 0f ? 0f : Mathf.Pow(2f, 10f * (t - 1f)),
            // OutExpo
            t => Mathf.Approximately(t, 1f) ? 1f : 1f - Mathf.Pow(2f, -10f * t),
            // InOutExpo
            t =>
            {
                if (t == 0f) return 0f;
                if (Mathf.Approximately(t, 1f)) return 1f;
                return t < 0.5f
                    ? Mathf.Pow(2f, 20f * t - 10f) / 2f
                    : (2f - Mathf.Pow(2f, -20f * t + 10f)) / 2f;
            },

            // InCirc
            t => 1f - Mathf.Sqrt(1f - t * t), 
            // OutCirc
            t => { t -= 1f; return Mathf.Sqrt(1f - t * t); }, 
            // InOutCirc
            t => t < 0.5f
                ? (1f - Mathf.Sqrt(1f - 4f * t * t)) / 2f
                : (Mathf.Sqrt(1f - Mathf.Pow(-2f * t + 2f, 2f)) + 1f) / 2f,

            // InBack
            t =>
            {
                const float c1 = 1.70158f;
                return t * t * ((c1 + 1f) * t - c1);
            }, 
            // OutBack
            t =>
            {
                const float c1 = 1.70158f;
                t -= 1f;
                return t * t * ((c1 + 1f) * t + c1) + 1f;
            }, 
            // InOutBack
            t =>
            {
                const float c1 = 1.70158f;
                const float c2 = c1 * 1.525f;
                return t < 0.5f
                    ? (t * 2f * t * 2f * ((c2 + 1f) * t * 2f - c2)) / 2f
                    : ((t * 2f - 2f) * (t * 2f - 2f) * ((c2 + 1f) * (t * 2f - 2f) + c2) + 2f) / 2f;
            },

            // InElastic
            t =>
            {
                if (t == 0f || Mathf.Approximately(t, 1f)) return t;
                return -Mathf.Pow(2f, 10f * (t - 1f)) *
                       Mathf.Sin((t - 1.075f) * (2f * Mathf.PI) / 0.3f);
            }, 
            // OutElastic
            t =>
            {
                if (t == 0f || Mathf.Approximately(t, 1f)) return t;
                return Mathf.Pow(2f, -10f * t) *
                    Mathf.Sin((t - 0.075f) * (2f * Mathf.PI) / 0.3f) + 1f;
            }, 
            // InOutElastic
            t =>
            {
                if (t == 0f || Mathf.Approximately(t, 1f)) return t;
                t *= 2f;
                if (t < 1f)
                    return -0.5f * Mathf.Pow(2f, 10f * (t - 1f)) *
                           Mathf.Sin((t - 1.1125f) * (2f * Mathf.PI) / 0.45f);
                return Mathf.Pow(2f, -10f * (t - 1f)) *
                    Mathf.Sin((t - 1.1125f) * (2f * Mathf.PI) / 0.45f) * 0.5f + 1f;
            },

            // InBounce
            t => 1f - OutBounce(1f - t),
            // OutBounce
            OutBounce, 
            // InOutBounce
            t => t < 0.5f
                ? (1f - OutBounce(1f - t * 2f)) / 2f
                : (1f + OutBounce(t * 2f - 1f)) / 2f
        };

        private static float OutBounce(float t)
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;
            
            switch (t)
            {
                case < 1f / d1:
                    return n1 * t * t;
                case < 2f / d1:
                    t -= 1.5f / d1; return n1 * t * t + 0.75f;
                case < 2.5f / d1:
                    t -= 2.25f / d1; return n1 * t * t + 0.9375f;
                default:
                    t -= 2.625f / d1; return n1 * t * t + 0.984375f;
            }
        }

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