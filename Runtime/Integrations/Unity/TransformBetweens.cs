using BetweenRedKit.Core;
using UnityEngine;

namespace BetweenRedKit.Integrations.Unity
{
    public abstract class TransformBetweenBase : IBetweenTarget
    {
        protected readonly Transform Tr;
        protected bool HasTransform => Tr != null;
        public bool IsAlive => Tr != null;
        public object Key => Tr;

        protected TransformBetweenBase(Transform tr) => Tr = tr;

        public abstract void CaptureStart();
        public abstract void Apply(float eased);
        public abstract void CompleteImmediately();
    }

    public sealed class PositionBetween : TransformBetweenBase
    {
        private Vector3 _start, _end;
        public PositionBetween(Transform tr, Vector3 end) : base(tr) => _end = end;
        public override void CaptureStart() => _start = Tr.position;
        public override void Apply(float eased) => Tr.position = Vector3.LerpUnclamped(_start, _end, eased);
        public override void CompleteImmediately() => Tr.position = _end;
    }

    public sealed class RotationBetween : TransformBetweenBase
    {
        private Quaternion _start, _end;
        public RotationBetween(Transform tr, Quaternion end) : base(tr) => _end = end;
        public override void CaptureStart() => _start = Tr.rotation;
        public override void Apply(float eased) => Tr.rotation = Quaternion.SlerpUnclamped(_start, _end, eased);
        public override void CompleteImmediately() => Tr.rotation = _end;
    }

    public sealed class ScaleBetween : TransformBetweenBase
    {
        private Vector3 _start, _end;
        public ScaleBetween(Transform tr, Vector3 end) : base(tr) => _end = end;
        public override void CaptureStart() => _start = Tr.localScale;
        public override void Apply(float eased) => Tr.localScale = Vector3.LerpUnclamped(_start, _end, eased);
        public override void CompleteImmediately() => Tr.localScale = _end;
    }
}