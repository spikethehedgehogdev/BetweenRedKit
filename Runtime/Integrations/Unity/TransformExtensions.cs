using System;
using BetweenRedKit.Core;
using BetweenRedKit.Easing;
using UnityEngine;

namespace BetweenRedKit.Integrations.Unity
{
    /// <summary>
    /// Provides extension methods for tweening <see cref="Transform"/> components.
    /// </summary>
    /// <remarks>
    /// All methods create lightweight, allocation-free tween targets managed by <see cref="BetweenProcessor"/>.
    /// Designed as a zero-GC alternative to coroutine-based or reflection-driven tween systems.
    /// </remarks>
    public static class TransformExtensions
    {
        #region Position

        /// <summary>
        /// Starts a tween that interpolates <see cref="Transform.position"/> toward the specified target.
        /// </summary>
        /// <param name="tr">The transform to animate.</param>
        /// <param name="p">The tween processor instance.</param>
        /// <param name="end">The target world position.</param>
        /// <param name="duration">Tween duration, in seconds.</param>
        /// <param name="ease">Easing function type.</param>
        /// <param name="onComplete">Optional callback invoked when the tween finishes.</param>
        /// <returns>A <see cref="BetweenHandle"/> referencing the created tween.</returns>
        public static BetweenHandle MoveTo(this Transform tr, BetweenProcessor p, Vector3 end, float duration, EaseType ease = EaseType.Linear, Action onComplete = null)
            => p.Create(new PositionBetween(tr, end), duration, ease, onComplete);
        
        /// <summary>
        /// Starts a tween that interpolates <see cref="Transform.localPosition"/> toward the specified target.
        /// </summary>
        /// <param name="tr">The transform to animate.</param>
        /// <param name="p">The tween processor instance.</param>
        /// <param name="end">The target local position.</param>
        /// <param name="duration">Tween duration, in seconds.</param>
        /// <param name="ease">Easing function type.</param>
        /// <param name="onComplete">Optional callback invoked when the tween finishes.</param>
        /// <returns>A <see cref="BetweenHandle"/> referencing the created tween.</returns>
        public static BetweenHandle LocalMoveTo(this Transform tr, BetweenProcessor p, Vector3 end, float duration,
            EaseType ease = EaseType.Linear, Action onComplete = null)
            => p.Create(new LocalPositionBetween(tr, end), duration, ease, onComplete);
        
        #endregion
        
        #region Rotation
        
        /// <summary>
        /// Starts a tween that interpolates <see cref="Transform.rotation"/> toward the specified target.
        /// </summary>
        /// <param name="tr">The transform to animate.</param>
        /// <param name="p">The tween processor instance.</param>
        /// <param name="end">The target global rotation.</param>
        /// <param name="duration">Tween duration, in seconds.</param>
        /// <param name="ease">Easing function type.</param>
        /// <param name="onComplete">Optional callback invoked when the tween finishes.</param>
        /// <returns>A <see cref="BetweenHandle"/> referencing the created tween.</returns>
        public static BetweenHandle RotateTo(this Transform tr, BetweenProcessor p, Quaternion end, float duration, EaseType ease = EaseType.Linear, Action onComplete = null)
            => p.Create(new RotationBetween(tr, end), duration, ease, onComplete);
        
        /// <summary>
        /// Starts a tween that interpolates <see cref="Transform.localRotation"/> toward the specified target.
        /// </summary>
        /// <param name="tr">The transform to animate.</param>
        /// <param name="p">The tween processor instance.</param>
        /// <param name="end">The target local rotation.</param>
        /// <param name="duration">Tween duration, in seconds.</param>
        /// <param name="ease">Easing function type.</param>
        /// <param name="onComplete">Optional callback invoked when the tween finishes.</param>
        /// <returns>A <see cref="BetweenHandle"/> referencing the created tween.</returns>
        public static BetweenHandle LocalRotateTo(this Transform tr, BetweenProcessor p, Quaternion end, float duration,
            EaseType ease = EaseType.Linear, Action onComplete = null)
            => p.Create(new LocalRotationBetween(tr, end), duration, ease, onComplete);
        #endregion
        
        #region Scale
        
        /// <summary>
        /// Starts a tween that interpolates <see cref="Transform.localScale"/> toward the specified target.
        /// </summary>
        /// <param name="tr">The transform to animate.</param>
        /// <param name="p">The tween processor instance.</param>
        /// <param name="end">The target local scale.</param>
        /// <param name="duration">Tween duration, in seconds.</param>
        /// <param name="ease">Easing function type.</param>
        /// <param name="onComplete">Optional callback invoked when the tween finishes.</param>
        /// <returns>A <see cref="BetweenHandle"/> referencing the created tween.</returns>
        public static BetweenHandle ScaleTo(this Transform tr, BetweenProcessor p, Vector3 end, float duration, EaseType ease = EaseType.Linear, Action onComplete = null)
            => p.Create(new ScaleBetween(tr, end), duration, ease, onComplete);
        
        #endregion
    }
}