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
        
        #region Sine
        
        /// <summary>Sine ease-in.</summary>
        InSine,
        /// <summary>Sine ease-out.</summary>
        OutSine,
        /// <summary>Sine ease-in-out.</summary>
        InOutSine,
        
        #endregion

        #region Quadratic

        /// <summary>Quadratic ease-in.</summary>
        InQuad,
        /// <summary>Quadratic ease-out.</summary>
        OutQuad,
        /// <summary>Quadratic ease-in-out.</summary>
        InOutQuad,
        
        #endregion

        #region Cubic

        /// <summary>Cubic ease-in.</summary>
        InCubic,
        /// <summary>Cubic ease-out.</summary>
        OutCubic,
        /// <summary>Cubic ease-in-out.</summary>
        InOutCubic,

        #endregion

        #region Quartic

        /// <summary>Quartic ease-in.</summary>
        InQuart,
        /// <summary>Quartic ease-out.</summary>
        OutQuart,
        /// <summary>Quartic ease-in-out.</summary>
        InOutQuart,
        
        #endregion

        #region Quintic

        /// <summary>Quintic ease-in.</summary>
        InQuint,
        /// <summary>Quintic ease-out.</summary>
        OutQuint,
        /// <summary>Quintic ease-in-out.</summary>
        InOutQuint,

        #endregion
        
        #region Exponential

        /// <summary>Exponential ease-in.</summary>
        InExpo,
        /// <summary>Exponential ease-out.</summary>
        OutExpo,
        /// <summary>Exponential ease-in-out.</summary>
        InOutExpo,
        
        #endregion

        #region Circular

        /// <summary>Circular ease-in.</summary>
        InCirc,
        /// <summary>Circular ease-out.</summary>
        OutCirc,
        /// <summary>Circular ease-in-out.</summary>
        InOutCirc,

        #endregion

        #region Back
        
        /// <summary>Back ease-in.</summary>
        InBack,
        /// <summary>Back ease-out.</summary>
        OutBack,
        /// <summary>Back ease-in-out.</summary>
        InOutBack,

        #endregion

        #region Elastic

        /// <summary>Elastic ease-in.</summary>
        InElastic,
        /// <summary>Elastic ease-out.</summary>
        OutElastic,
        /// <summary>Elastic ease-in-out.</summary>
        InOutElastic,
        
        #endregion

        #region Bounce
        
        /// <summary>Bounce ease-in.</summary>
        InBounce,
        /// <summary>Bounce ease-out.</summary>
        OutBounce,
        /// <summary>Bounce ease-in-out.</summary>
        InOutBounce

        #endregion
    }
}