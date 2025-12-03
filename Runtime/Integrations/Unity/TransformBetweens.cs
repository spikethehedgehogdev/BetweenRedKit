using BetweenRedKit.Core;
using UnityEngine;

namespace BetweenRedKit.Integrations.Unity
{
    #region Base

    /// <summary>
    /// Base class for transform-based tween targets (<see cref="PositionBetween"/>, <see cref="RotationBetween"/>, <see cref="ScaleBetween"/>).
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the transform tween base.
        /// </summary>
        /// <param name="tr">The transform to animate.</param>
        protected TransformBetweenBase(Transform tr) => Tr = tr;

        #region IBetweenTarget

        /// <inheritdoc />
        public bool IsAlive => Tr != null;

        /// <inheritdoc />
        public object Key => Tr;

        /// <inheritdoc />
        public abstract void CaptureStart();

        /// <inheritdoc />
        public abstract void Apply(float eased);

        /// <inheritdoc />
        public abstract void CompleteImmediately();
        
        #endregion
    }
    
    #endregion

    #region Position

    /// <summary>
    /// Tween target that interpolates <see cref="Transform.position"/>.
    /// </summary>
    public sealed class PositionBetween : TransformBetweenBase
    {
        private Vector3 _start, _end;

        /// <summary>
        /// Creates a new position tween target.
        /// </summary>
        /// <param name="tr">The transform to animate.</param>
        /// <param name="end">The target world position.</param>
        public PositionBetween(Transform tr, Vector3 end) : base(tr) => _end = end;

        #region TransformBetweenBase

        /// <inheritdoc />
        public override void CaptureStart() => _start = Tr.position;

        /// <inheritdoc />
        public override void Apply(float eased) => Tr.position = Vector3.LerpUnclamped(_start, _end, eased);

        /// <inheritdoc />
        public override void CompleteImmediately() => Tr.position = _end;
        
        #endregion
    }
    
    /// <summary>
    /// Tween target that interpolates <see cref="Transform.localPosition"/>.
    /// </summary>
    public sealed class LocalPositionBetween : TransformBetweenBase
    {
        private Vector3 _start, _end;

        public LocalPositionBetween(Transform tr, Vector3 end) : base(tr) => _end = end;

        #region TransformBetweenBase

        /// <inheritdoc />
        public override void CaptureStart() => _start = Tr.localPosition;

        /// <inheritdoc />
        public override void Apply(float eased) => Tr.localPosition = Vector3.LerpUnclamped(_start, _end, eased);

        /// <inheritdoc />
        public override void CompleteImmediately() => Tr.localPosition = _end;

        #endregion
    }
    
    #endregion

    #region Rotation
    
    /// <summary>
    /// Tween target that interpolates <see cref="Transform.rotation"/>.
    /// </summary>
    public sealed class RotationBetween : TransformBetweenBase
    {
        private Quaternion _start, _end;

        /// <summary>
        /// Creates a new rotation tween target.
        /// </summary>
        /// <param name="tr">The transform to animate.</param>
        /// <param name="end">The target rotation.</param>
        public RotationBetween(Transform tr, Quaternion end) : base(tr) => _end = end;

        #region TransformBetweenBase

        /// <inheritdoc />
        public override void CaptureStart() => _start = Tr.rotation;

        /// <inheritdoc />
        public override void Apply(float eased) => Tr.rotation = Quaternion.SlerpUnclamped(_start, _end, eased);

        /// <inheritdoc />
        public override void CompleteImmediately() => Tr.rotation = _end;
        
        #endregion
    }

    /// <summary>
    /// Tween target that interpolates <see cref="Transform.localRotation"/>.
    /// </summary>
    public sealed class LocalRotationBetween : TransformBetweenBase
    {
        private Quaternion _start, _end;

        public LocalRotationBetween(Transform tr, Quaternion end) : base(tr) => _end = end;

        #region TransformBetweenBase
        
        /// <inheritdoc />
        public override void CaptureStart() => _start = Tr.localRotation;

        /// <inheritdoc />
        public override void Apply(float eased) => Tr.localRotation = Quaternion.SlerpUnclamped(_start, _end, eased);

        /// <inheritdoc />
        public override void CompleteImmediately() => Tr.localRotation = _end;

        #endregion
    }

    #endregion

    #region Scale

    /// <summary>
    /// Tween target that interpolates <see cref="Transform.localScale"/>.
    /// </summary>
    public sealed class ScaleBetween : TransformBetweenBase
    {
        private Vector3 _start, _end;

        /// <summary>
        /// Creates a new scale tween target.
        /// </summary>
        /// <param name="tr">The transform to animate.</param>
        /// <param name="end">The target local scale.</param>
        public ScaleBetween(Transform tr, Vector3 end) : base(tr) => _end = end;

        #region TransformBetweenBase

        /// <inheritdoc />
        public override void CaptureStart() => _start = Tr.localScale;

        /// <inheritdoc />
        public override void Apply(float eased) => Tr.localScale = Vector3.LerpUnclamped(_start, _end, eased);

        /// <inheritdoc />
        public override void CompleteImmediately() => Tr.localScale = _end;
        
        #endregion
    }
    
    #endregion
}
