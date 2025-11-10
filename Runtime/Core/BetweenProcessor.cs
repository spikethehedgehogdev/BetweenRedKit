using System;
using System.Collections.Generic;
using BetweenRedKit.Easing;

namespace BetweenRedKit.Core
{
    public sealed class BetweenProcessor : IDisposable
    {
        private BetweenSlot[] _pool;
        private int[] _active;
        private int _activeCount;
        private readonly Stack<int> _free;

        private readonly ICapacityPolicy _capacityPolicy;
        private readonly IEasingProvider _easingProvider;
        private readonly ITimeProvider _timeProvider;

        public int ActiveCount => _activeCount;
        public int Capacity => _pool.Length;
        public int Expansions { get; private set; }

        public BetweenProcessor(
            int initialCapacity = 256,
            ICapacityPolicy capacityPolicy = null,
            IEasingProvider easingProvider = null,
            ITimeProvider timeProvider = null)
        {
            if (initialCapacity < 1)
                initialCapacity = 4;

            _capacityPolicy = capacityPolicy ?? new DoublingCapacityPolicy();
            _easingProvider  = easingProvider  ?? new DefaultEasingProvider();
            _timeProvider    = timeProvider    ?? new UnityTimeProvider();

            _pool   = new BetweenSlot[initialCapacity];
            _active = new int[initialCapacity];
            _free   = new Stack<int>(initialCapacity);

            for (var i = initialCapacity - 1; i >= 0; i--)
                _free.Push(i);
        }

        public BetweenHandle Create(
            IBetweenTarget target,
            float duration,
            EaseType ease = EaseType.Linear,
            Action onComplete = null)
        {
            if (target is not { IsAlive: true })
                return default;

            var easeFunc = _easingProvider.Get(ease) ?? (t => t);
            var id = Allocate();
            if (id < 0)
                return default;

            var version = InitSlot(id, target, Math.Max(duration, 0.0001f), easeFunc, onComplete);
            return new BetweenHandle(id, version, this);
        }

        private int Allocate()
        {
            if (_free.Count > 0)
                return _free.Pop();

            var oldCap = _pool.Length;
            var newCap = _capacityPolicy.Grow(oldCap);
            if (newCap <= oldCap)
                return -1;

            Array.Resize(ref _pool, newCap);
            Array.Resize(ref _active, newCap);

            for (var i = newCap - 1; i >= oldCap; i--)
                _free.Push(i);

            Expansions++;
            return _free.Pop();
        }

        private int InitSlot(int idx, IBetweenTarget target, float duration, Func<float, float> ease, Action onComplete)
        {
            ref var s = ref _pool[idx];
            s.Active = true;
            s.Target = target;
            s.Duration = duration;
            s.Ease = ease;
            s.OnComplete = onComplete;
            s.Time = 0f;
            s.Paused = false;
            s.Version++;

            target.CaptureStart();

            _active[_activeCount++] = idx;
            return s.Version;
        }

        public void Tick()
        {
            var dt = _timeProvider.DeltaTime;
            if (dt <= 0f || _activeCount == 0)
                return;

            for (var k = 0; k < _activeCount;)
            {
                var i = _active[k];
                ref var s = ref _pool[i];

                if (!s.Active)
                { RemoveAtDense(k, i, ref s); continue; }

                var target = s.Target;
                if (target is not { IsAlive: true })
                { RemoveAtDense(k, i, ref s); continue; }

                if (s.Paused)
                { k++; continue; }

                s.Time += dt;
                var p = s.Time / s.Duration;

                if (p < 1f)
                {
                    var eased = s.Ease?.Invoke(p) ?? p;
                    target.Apply(eased);
                    k++;
                    continue;
                }

                target.Apply(1f);
                target.CompleteImmediately();
                s.OnComplete?.Invoke();
                RemoveAtDense(k, i, ref s);
            }
        }

        private void RemoveAtDense(int k, int index, ref BetweenSlot s)
        {
            Reset(ref s);
            _free.Push(index);

            var last = --_activeCount;
            if (k < last)
                _active[k] = _active[last];
        }

        private static void Reset(ref BetweenSlot s)
        {
            s.Active = false;
            s.Target = null;
            s.OnComplete = null;
            s.Ease = null;
            s.Paused = false;
            s.Time = 0f;
            s.Duration = 0f;
        }

        internal bool IsActive(int id, int version) =>
            (uint)id < (uint)_pool.Length && _pool[id].Active && _pool[id].Version == version;

        internal bool TryGetSlot(int id, out BetweenSlot slot)
        {
            if ((uint)id < (uint)_pool.Length)
            {
                slot = _pool[id];
                return true;
            }
            slot = default;
            return false;
        }

        public void Stop(int id, bool complete = false)
        {
            if ((uint)id >= (uint)_pool.Length) return;
            ref var s = ref _pool[id];
            if (!s.Active) return;

            if (complete && s.Target?.IsAlive == true)
            {
                s.Target.CompleteImmediately();
                s.OnComplete?.Invoke();
            }

            for (var k = 0; k < _activeCount; k++)
                if (_active[k] == id)
                {
                    RemoveAtDense(k, id, ref s);
                    return;
                }

            Reset(ref s);
            _free.Push(id);
        }

        public void Pause(int id, bool pause)
        {
            if ((uint)id < (uint)_pool.Length)
                _pool[id].Paused = pause;
        }

        public void PauseAll(bool pause = true)
        {
            for (var n = 0; n < _activeCount; n++)
                _pool[_active[n]].Paused = pause;
        }

        public void RegisterOnComplete(int id, Action callback)
        {
            if ((uint)id >= (uint)_pool.Length)
                return;

            ref var s = ref _pool[id];
            if (!s.Active)
            {
                callback?.Invoke();
                return;
            }

            s.OnComplete += callback;
        }

        public bool IsActive(int id) =>
            (uint)id < (uint)_pool.Length && _pool[id].Active;

        public void CullInvalidTargets()
        {
            for (var k = 0; k < _activeCount;)
            {
                var i = _active[k];
                ref var s = ref _pool[i];
                if (!s.Active || s.Target is not { IsAlive: true })
                {
                    RemoveAtDense(k, i, ref s);
                    continue;
                }
                k++;
            }
        }

        public void Dispose()
        {
            for (var k = 0; k < _activeCount; k++)
                Reset(ref _pool[_active[k]]);

            Array.Clear(_active, 0, _activeCount);
            _activeCount = 0;

            _pool = Array.Empty<BetweenSlot>();
            _active = Array.Empty<int>();
            _free.Clear();
        }
    }
}
