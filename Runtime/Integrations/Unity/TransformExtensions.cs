using System;
using BetweenRedKit.Core;
using BetweenRedKit.Easing;
using UnityEngine;

namespace BetweenRedKit.Integrations.Unity
{
    public static class TransformExtensions
    {
        public static BetweenHandle MoveTo(this Transform tr, BetweenProcessor p, Vector3 end, float duration, EaseType ease = EaseType.Linear, Action onComplete = null)
            => p.Create(new PositionBetween(tr, end), duration, ease, onComplete);

        public static BetweenHandle RotateTo(this Transform tr, BetweenProcessor p, Quaternion end, float duration, EaseType ease = EaseType.Linear, Action onComplete = null)
            => p.Create(new RotationBetween(tr, end), duration, ease, onComplete);

        public static BetweenHandle ScaleTo(this Transform tr, BetweenProcessor p, Vector3 end, float duration, EaseType ease = EaseType.Linear, Action onComplete = null)
            => p.Create(new ScaleBetween(tr, end), duration, ease, onComplete);
    }
}