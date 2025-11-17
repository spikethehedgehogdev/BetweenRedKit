using System;
using System.Collections.Generic;
using BetweenRedKit.Easing;

namespace BetweenRedKit.Core
{
    /// <summary>
    /// Core tween processor responsible for allocating, updating, and recycling active tweens.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The processor manages a dense array of active slots (<see cref="BetweenSlot"/>),
    /// ensuring zero-GC updates and constant-time removal through <c>dense-remove</c>.
    /// </para>
    /// <para>
    /// The system is designed for deterministic, low-level control, independent from Unity's
    /// <see cref="UnityEngine.MonoBehaviour"/> lifecycle. All updates are driven manually via <see cref="Tick"/>.
    /// </para>
    /// </remarks>
    public sealed class BetweenProcessor : IDisposable
    {
        private BetweenSlot[] _pool;
        private int[] _active;
        private int _activeCount;
        private readonly Stack<int> _free;

        private readonly ICapacityPolicy _capacityPolicy;
        private readonly IEasingProvider _easingProvider;
        private readonly ITimeProvider _timeProvider;

        /// <summary>
        /// Gets the current number of active tweens.
        /// </summary>
        public int ActiveCount => _activeCount;

        /// <summary>
        /// Gets the total number of slots allocated in the pool.
        /// </summary>
        public int Capacity => _pool.Length;

        /// <summary>
        /// Gets the number of times the internal pool has expanded.
        /// </summary>
        public int Expansions { get; private set; }

        /// <summary>
        /// Creates a new tween processor with the specified configuration.
        /// </summary>
        /// <param name="initialCapacity">Initial number of preallocated tween slots.</param>
        /// <param name="capacityPolicy">Defines how the pool grows when full (default: doubling).</param>
        /// <param name="easingProvider">Provides easing functions (default: <see cref="DefaultEasingProvider"/>).</param>
        /// <param name="timeProvider">Provides delta time for updates (default: <see cref="UnityTimeProvider"/>).</param>
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

        /// <summary>
        /// Creates a new tween for the given target.
        /// </summary>
        /// <param name="target">Tween target implementing <see cref="IBetweenTarget"/>.</param>
        /// <param name="duration">Tween duration, in seconds.</param>
        /// <param name="ease">Easing type for interpolation.</param>
        /// <param name="onComplete">Optional callback executed upon completion.</param>
        /// <returns>A <see cref="BetweenHandle"/> referencing the created tween.</returns>
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
        
        /// <summary>
        /// Updates all active tweens using the provided <see cref="ITimeProvider"/>.
        /// Should be called once per frame or simulation step.
        /// </summary>
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

        /// <summary>
        /// Stops the tween with the specified ID.
        /// </summary>
        /// <param name="id">Tween ID to stop.</param>
        /// <param name="complete">
        /// If true, applies the final value and triggers completion callbacks.
        /// If false, stops the tween immediately.
        /// </param>
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
        
        /// <summary>
        /// Pauses or resumes the tween with the given ID.
        /// </summary>
        /// <param name="id">Tween identifier.</param>
        /// <param name="pause">True to pause, false to resume.</param>
        public void Pause(int id, bool pause)
        {
            if ((uint)id < (uint)_pool.Length)
                _pool[id].Paused = pause;
        }
        
        /// <summary>
        /// Pauses or resumes all currently active tweens.
        /// </summary>
        /// <param name="pause">True to pause, false to resume.</param>
        public void PauseAll(bool pause = true)
        {
            for (var n = 0; n < _activeCount; n++)
                _pool[_active[n]].Paused = pause;
        }

        /// <summary>
        /// Registers a completion callback for an active tween.
        /// </summary>
        /// <param name="id">Tween identifier.</param>
        /// <param name="callback">Callback to invoke upon completion.</param>
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
        
        /// <summary>
        /// Removes all inactive or destroyed tween targets from the processor.
        /// </summary>
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

        /// <summary>
        /// Clears and releases all resources used by this processor.
        /// </summary>
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
        
        /// <summary>
        /// Returns whether a tween with the given identifier is currently active.
        /// </summary>
        public bool IsActive(int id) =>
            (uint)id < (uint)_pool.Length && _pool[id].Active;
        
        /// <summary>
        /// Internal helper for version-based validation of active tween slots.
        /// </summary>
        internal bool IsActive(int id, int version) =>
            (uint)id < (uint)_pool.Length && _pool[id].Active && _pool[id].Version == version;

        /// <summary>
        /// Attempts to retrieve a copy of the specified tween slot.
        /// For internal debugging, profiling, or inspection only.
        /// </summary>
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
    }
}
