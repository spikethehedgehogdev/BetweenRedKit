namespace BetweenRedKit.Core
{
    /// <summary>
    /// Defines a tweenable target that can be processed by <see cref="BetweenProcessor"/>.
    /// </summary>
    /// <remarks>
    /// Implementations must be lightweight and allocation-free.
    /// A target is responsible for applying interpolated values to its own data source.
    /// <para>
    /// The processor will:
    /// <list type="number">
    /// <item><description>Call <see cref="CaptureStart"/> once before the tween begins.</description></item>
    /// <item><description>Call <see cref="Apply"/> every frame with an eased progress value (0..1).</description></item>
    /// <item><description>Call <see cref="CompleteImmediately"/> at completion or interruption.</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public interface IBetweenTarget
    {
        /// <summary>
        /// Captures the current source value to be used as the tween starting point.
        /// Called once when the tween is initialized.
        /// </summary>
        void CaptureStart();

        /// <summary>
        /// Applies the interpolated value for the given eased progress.
        /// </summary>
        /// <param name="eased">Eased progress value (0–1).</param>
        void Apply(float eased);

        /// <summary>
        /// Immediately applies the final value of the tween.
        /// Called when the tween completes or is forcibly stopped with completion.
        /// </summary>
        void CompleteImmediately();

        /// <summary>
        /// Indicates whether the target is still valid and can receive updates.
        /// </summary>
        bool IsAlive { get; }

        /// <summary>
        /// Unique object key used for identification and duplicate prevention.
        /// </summary>
        object Key { get; }
    }
}