using System;

namespace BetweenRedKit.Core
{
    public interface ICapacityPolicy
    {
        int Grow(int currentCapacity);
    }

    public sealed class DoublingCapacityPolicy : ICapacityPolicy
    {
        private readonly int _max;
        public DoublingCapacityPolicy(int max = 1 << 20) => _max = max;
        public int Grow(int current) => Math.Min(_max, Math.Max(4, current * 2));
    }

    public sealed class FixedCapacityPolicy : ICapacityPolicy
    {
        public int Grow(int current) => current;
    }
}