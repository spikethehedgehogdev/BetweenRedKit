using System;
using System.Collections.Generic;

namespace BetweenRedKit.Easing
{
    public interface IEasingProvider
    {
        Func<float, float> Get(EaseType type);
        void Register(EaseType type, Func<float, float> func);
    }

    public sealed class DefaultEasingProvider : IEasingProvider
    {
        private static readonly Func<float, float>[] BuiltIn = {
            t => t,
            t => t * t,
            t => 1 - (1 - t) * (1 - t),
            t => t < 0.5f ? 2*t*t : 1 - (float)Math.Pow(-2*t+2,2)/2,
            t => t*t*t*t*t,
            t => 1 - (float)Math.Pow(1 - t, 5),
            t => t < 0.5f ? 16*t*t*t*t*t : 1 - (float)Math.Pow(-2*t+2,5)/2
        };

        private readonly Dictionary<EaseType, Func<float, float>> _custom = new();

        public Func<float, float> Get(EaseType type)
        {
            return _custom.TryGetValue(type, out var f) ? f : BuiltIn[(int)type];
        }

        public void Register(EaseType type, Func<float, float> func)
        {
            _custom[type] = func ?? throw new ArgumentNullException(nameof(func));
        }
    }
}
