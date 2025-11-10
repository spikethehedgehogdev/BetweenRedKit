using UnityEngine;

namespace BetweenRedKit.Core
{
    public interface ITimeProvider
    {
        float DeltaTime { get; }
    }

    public sealed class UnityTimeProvider : ITimeProvider
    {
        public float DeltaTime => Time.deltaTime;
    }

    public sealed class ManualTimeProvider : ITimeProvider
    {
        public float DeltaTime { get; set; }
    }
}