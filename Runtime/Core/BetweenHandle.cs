using System;

namespace BetweenRedKit.Core
{
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

        public BetweenHandle OnComplete(Action callback)
        {
            if (IsValid)
                _processor.RegisterOnComplete(_id, callback);
            else
                callback?.Invoke();
            return this;
        }

        public BetweenHandle Pause(bool value = true)
        {
            if (IsValid)
                _processor.Pause(_id, value);
            return this;
        }

        public void Resume()
        {
            if (IsValid)
                _processor.Pause(_id, false);
        }

        public void Stop(bool complete = false)
        {
            if (IsValid)
                _processor.Stop(_id, complete);
        }

        public bool IsActive => IsValid;
        public int Id => _id;
        public int Version => _version;
    }
}