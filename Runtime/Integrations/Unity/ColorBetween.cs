using BetweenRedKit.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BetweenRedKit.Integrations.Unity
{
    /// <summary>
    /// A tween target that interpolates the <see cref="Graphic.color"/> property.
    /// Implements a zero-allocation, direct-access color transition for UI elements.
    /// </summary>
    /// <remarks>
    /// This implementation uses <see cref="Color.LerpUnclamped(Color, Color, float)"/> and
    /// does not allocate memory at runtime. Intended for precise, low-level control
    /// rather than abstracted convenience methods.
    /// </remarks>
    public sealed class ColorBetween : IBetweenTarget
    {
        private readonly Graphic _graphic;
        private readonly Color _end;
        
        private Color _start;

        /// <inheritdoc />
        public bool IsAlive => _graphic != null;

        /// <inheritdoc />
        public object Key => _graphic;

        /// <summary>
        /// Creates a new tween target for interpolating <see cref="Graphic.color"/>.
        /// </summary>
        /// <param name="graphic">UI element to animate.</param>
        /// <param name="end">Target color.</param>
        public ColorBetween(Graphic graphic, Color end)
        {
            _graphic = graphic;
            _end = end;
        }

        /// <inheritdoc />
        public void CaptureStart() => _start = _graphic.color;

        /// <inheritdoc />
        public void Apply(float eased) => _graphic.color = Color.LerpUnclamped(_start, _end, eased);

        /// <inheritdoc />
        public void CompleteImmediately() => _graphic.color = _end;
    }
}