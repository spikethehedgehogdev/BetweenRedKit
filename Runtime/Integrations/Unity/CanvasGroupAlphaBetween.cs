using BetweenRedKit.Core;
using UnityEngine;

namespace BetweenRedKit.Integrations.Unity
{
    /// <summary>
    /// A tween target that interpolates <see cref="CanvasGroup.alpha"/>.
    /// Designed for fading UI groups in or out without GC allocations.
    /// </summary>
    /// <remarks>
    /// Separated from <see cref="ColorBetween"/> intentionally to preserve
    /// zero-GC design and avoid runtime dispatch between heterogeneous targets.
    /// The difference between color alpha and CanvasGroup alpha is semantic:
    /// the latter affects the entire UI group, not a single element.
    /// </remarks>
    public sealed class CanvasGroupAlphaBetween : IBetweenTarget
    {
        private readonly CanvasGroup _group;
        private readonly float _end;
        private float _start;

        /// <inheritdoc />
        public bool IsAlive => _group != null;

        /// <inheritdoc />
        public object Key => _group;

        /// <summary>
        /// Creates a new tween target for interpolating <see cref="CanvasGroup.alpha"/>.
        /// </summary>
        /// <param name="group">Target CanvasGroup.</param>
        /// <param name="end">Target alpha value.</param>
        public CanvasGroupAlphaBetween(CanvasGroup group, float end)
        {
            _group = group;
            _end = end;
        }

        /// <inheritdoc />
        public void CaptureStart() => _start = _group.alpha;

        /// <inheritdoc />
        public void Apply(float eased) => _group.alpha = Mathf.LerpUnclamped(_start, _end, eased);

        /// <inheritdoc />
        public void CompleteImmediately() => _group.alpha = _end;
    }
}