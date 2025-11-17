namespace BetweenRedKit.Easing
{
    /// <summary>
    /// Specifies standard easing curve types used by the tween processor.
    /// </summary>
    /// <remarks>
    /// These values map directly to functions provided by <see cref="DefaultEasingProvider"/>.
    /// Custom easing types can also be registered via <see cref="IEasingProvider.Register"/>.
    /// </remarks>
    public enum EaseType
    {
        /// <summary>Linear interpolation with constant speed.</summary>
        Linear,

        /// <summary>Quadratic acceleration at the start.</summary>
        InQuad,

        /// <summary>Quadratic deceleration towards the end.</summary>
        OutQuad,

        /// <summary>Quadratic ease-in followed by ease-out.</summary>
        InOutQuad,

        /// <summary>Strong quintic acceleration at the start.</summary>
        InQuint,

        /// <summary>Strong quintic deceleration at the end.</summary>
        OutQuint,

        /// <summary>Symmetrical quintic ease-in/out curve.</summary>
        InOutQuint
    }
}