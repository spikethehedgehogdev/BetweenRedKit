using System;

namespace BetweenRedKit.Core
{
    /// <summary>
    /// Represents a lightweight reference to a tween managed by <see cref="BetweenProcessor"/>.
    /// </summary>
    /// <remarks>
    /// A handle is value-type safe: it stores both the tween <see cref="Id"/> and a version counter,
    /// preventing access to recycled slots. 
    /// All operations are zero-GC and directly delegate to the owning processor.
    /// </remarks>
    public readonly struct BetweenHandle
    {
        private readonly int _id;
        private readonly int _version;
        private readonly BetweenProcessor _processor;

        internal BetweenHandle(int id, int version, BetweenProcessor processor)
        {
            _id = id;
            _version = version;
            _processor = processor;
        }

        private bool IsValid =>
            _processor != null && _processor.IsActive(_id, _version);

        /// <summary>
        /// Registers a callback that will be invoked when the tween completes.
        /// </summary>
        /// <param name="callback">The callback to execute on completion.</param>
        /// <returns>The same handle instance for chaining.</returns>
        public BetweenHandle OnComplete(Action callback)
        {
            if (IsValid)
                _processor.RegisterOnComplete(_id, callback);
            else
                callback?.Invoke();
            return this;
        }

        /// <summary>
        /// Pauses or resumes the tween associated with this handle.
        /// </summary>
        /// <param name="value">If true, pauses the tween; if false, resumes it.</param>
        /// <returns>The same handle instance for chaining.</returns>
        public BetweenHandle Pause(bool value = true)
        {
            if (IsValid)
                _processor.Pause(_id, value);
            return this;
        }

        /// <summary>
        /// Resumes the tween if it is currently paused.
        /// </summary>
        public void Resume()
        {
            if (IsValid)
                _processor.Pause(_id, false);
        }

        /// <summary>
        /// Stops the tween, optionally completing it immediately.
        /// </summary>
        /// <param name="complete">
        /// If true, applies the final state and invokes completion callbacks.
        /// If false, simply cancels the tween.
        /// </param>
        public void Stop(bool complete = false)
        {
            if (IsValid)
                _processor.Stop(_id, complete);
        }

        /// <summary>
        /// Gets whether the tween is still active and valid.
        /// </summary>
        public bool IsActive => IsValid;

        /// <summary>
        /// Unique slot identifier within the owning <see cref="BetweenProcessor"/>.
        /// </summary>
        public int Id => _id;

        /// <summary>
        /// Version number used for validation against recycled slots.
        /// </summary>
        public int Version => _version;
    }
}