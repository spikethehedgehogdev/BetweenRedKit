using System;
using BetweenRedKit.Core;
using BetweenRedKit.Easing;
using UnityEngine;
using UnityEngine.UI;

namespace BetweenRedKit.Integrations.Unity
{
    /// <summary>
    /// Provides extension methods for tweening <see cref="Graphic"/> and <see cref="CanvasGroup"/> components.
    /// </summary>
    /// <remarks>
    /// The API intentionally separates color and alpha tweens into specific methods
    /// to maintain performance and avoid runtime dispatch. Each method creates
    /// a dedicated <see cref="IBetweenTarget"/> instance with zero allocations.
    /// </remarks>
    public static class GraphicExtensions
    {
        /// <summary>
        /// Starts a tween interpolating the <see cref="Graphic.color"/> property.
        /// </summary>
        /// <param name="graphic">UI element to animate.</param>
        /// <param name="p">Tween processor.</param>
        /// <param name="end">Target color value.</param>
        /// <param name="duration">Duration of the tween, in seconds.</param>
        /// <param name="ease">Easing function type.</param>
        /// <param name="onComplete">Optional callback executed when the tween finishes.</param>
        /// <returns>A <see cref="BetweenHandle"/> referencing the tween instance.</returns>
        public static BetweenHandle ColorTo(this Graphic graphic, BetweenProcessor p, Color end, float duration,
            EaseType ease = EaseType.Linear, Action onComplete = null)
            => p.Create(new ColorBetween(graphic, end), duration, ease, onComplete);

        /// <summary>
        /// Starts a tween interpolating <see cref="CanvasGroup.alpha"/>.
        /// </summary>
        /// <param name="group">CanvasGroup component to animate.</param>
        /// <param name="p">Tween processor.</param>
        /// <param name="end">Target alpha value.</param>
        /// <param name="duration">Duration of the tween, in seconds.</param>
        /// <param name="ease">Easing function type.</param>
        /// <param name="onComplete">Optional callback executed when the tween finishes.</param>
        /// <returns>A <see cref="BetweenHandle"/> referencing the tween instance.</returns>
        public static BetweenHandle FadeTo(this CanvasGroup group, BetweenProcessor p, float end, float duration,
            EaseType ease = EaseType.Linear, Action onComplete = null)
            => p.Create(new CanvasGroupAlphaBetween(group, end), duration, ease, onComplete);
    }
}