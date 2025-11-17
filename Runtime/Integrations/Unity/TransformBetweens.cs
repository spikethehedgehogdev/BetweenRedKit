using BetweenRedKit.Core;
using UnityEngine;

namespace BetweenRedKit.Integrations.Unity
{
    /// <summary>
    /// Base class for transform-based tween targets (<see cref="PositionBetween"/>, <see cref="RotationBetween"/>, <see cref="ScaleBetween"/>).
    /// </summary>
    /// <remarks>
    /// Implements the <see cref="IBetweenTarget"/> interface and provides common lifetime validation
    /// and transform reference handling for zero-GC interpolation of <see cref="Transform"/> properties.
    /// </remarks>
    public abstract class TransformBetweenBase : IBetweenTarget
    {
        /// <summary>
        /// The associated transform being animated.
        /// </summary>
        protected readonly Transform Tr;

        /// <summary>
        /// Indicates whether the target transform reference is valid.
        /// </summary>
        protected bool HasTransform => Tr != null;

        /// <inheritdoc />
        public bool IsAlive => Tr != null;

        /// <inheritdoc />
        public object Key => Tr;

        /// <summary>
        /// Initializes a new instance of the transform tween base.
        /// </summary>
        /// <param name="tr">The transform to animate.</param>
        protected TransformBetweenBase(Transform tr) => Tr = tr;

        /// <inheritdoc />
        public abstract void CaptureStart();

        /// <inheritdoc />
        public abstract void Apply(float eased);

        /// <inheritdoc />
        public abstract void CompleteImmediately();
    }

    /// <summary>
    /// Tween target that interpolates <see cref="Transform.position"/>.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="Vector3.LerpUnclamped(Vector3, Vector3, float)"/> for zero-GC position interpolation.
    /// </remarks>
    public sealed class PositionBetween : TransformBetweenBase
    {
        private Vector3 _start, _end;

        /// <summary>
        /// Creates a new position tween target.
        /// </summary>
        /// <param name="tr">The transform to animate.</param>
        /// <param name="end">The target world position.</param>
        public PositionBetween(Transform tr, Vector3 end) : base(tr) => _end = end;

        /// <inheritdoc />
        public override void CaptureStart() => _start = Tr.position;

        /// <inheritdoc />
        public override void Apply(float eased) => Tr.position = Vector3.LerpUnclamped(_start, _end, eased);

        /// <inheritdoc />
        public override void CompleteImmediately() => Tr.position = _end;
    }

    /// <summary>
    /// Tween target that interpolates <see cref="Transform.rotation"/>.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="Quaternion.SlerpUnclamped(Quaternion, Quaternion, float)"/> for smooth rotational transitions.
    /// </remarks>
    public sealed class RotationBetween : TransformBetweenBase
    {
        private Quaternion _start, _end;

        /// <summary>
        /// Creates a new rotation tween target.
        /// </summary>
        /// <param name="tr">The transform to animate.</param>
        /// <param name="end">The target rotation.</param>
        public RotationBetween(Transform tr, Quaternion end) : base(tr) => _end = end;

        /// <inheritdoc />
        public override void CaptureStart() => _start = Tr.rotation;

        /// <inheritdoc />
        public override void Apply(float eased) => Tr.rotation = Quaternion.SlerpUnclamped(_start, _end, eased);

        /// <inheritdoc />
        public override void CompleteImmediately() => Tr.rotation = _end;
    }

    /// <summary>
    /// Tween target that interpolates <see cref="Transform.localScale"/>.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="Vector3.LerpUnclamped(Vector3, Vector3, float)"/> for zero-GC local scale interpolation.
    /// </remarks>
    public sealed class ScaleBetween : TransformBetweenBase
    {
        private Vector3 _start, _end;

        /// <summary>
        /// Creates a new scale tween target.
        /// </summary>
        /// <param name="tr">The transform to animate.</param>
        /// <param name="end">The target local scale.</param>
        public ScaleBetween(Transform tr, Vector3 end) : base(tr) => _end = end;

        /// <inheritdoc />
        public override void CaptureStart() => _start = Tr.localScale;

        /// <inheritdoc />
        public override void Apply(float eased) => Tr.localScale = Vector3.LerpUnclamped(_start, _end, eased);

        /// <inheritdoc />
        public override void CompleteImmediately() => Tr.localScale = _end;
    }
}