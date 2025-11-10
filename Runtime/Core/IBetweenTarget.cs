namespace BetweenRedKit.Core
{
    public interface IBetweenTarget
    {
        void CaptureStart();
        void Apply(float eased);
        void CompleteImmediately();
        bool IsAlive { get; }
        object Key { get; }
    }
}