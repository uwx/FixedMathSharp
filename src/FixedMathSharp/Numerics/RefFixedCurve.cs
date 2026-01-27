using MessagePack;
using System;
using System.Linq;

#if NET8_0_OR_GREATER
using System.Text.Json.Serialization;
#endif

namespace FixedMathSharp
{
    /// <summary>
    /// A deterministic fixed-point curve that interpolates values between keyframes.
    /// Used for animations, physics calculations, and procedural data.
    /// </summary>
    public ref struct RefFixedCurve : IEquatable<RefFixedCurve>
    {
        public FixedCurveMode Mode { get; private set; }

        public ReadOnlySpan<FixedCurveKey> Keyframes { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RefFixedCurve"/> with a default linear interpolation mode.
        /// </summary>
        /// <param name="keyframes">The keyframes defining the curve.</param>
        public RefFixedCurve(ReadOnlySpan<FixedCurveKey> keyframes)
            : this(FixedCurveMode.Linear, keyframes) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RefFixedCurve"/> with a specified interpolation mode.
        /// </summary>
        /// <param name="mode">The interpolation method to use.</param>
        /// <param name="keyframes">The keyframes defining the curve.</param>
        public RefFixedCurve(FixedCurveMode mode, ReadOnlySpan<FixedCurveKey> keyframes)
        {
            // Throw if keyframes are not sorted by time
            for (var i = 1; i < keyframes.Length; i++)
            {
                if (keyframes[i].Time < keyframes[i - 1].Time)
                    ThrowKeyframesNotInOrder();
            }

            Keyframes = keyframes;
            Mode = mode;
            return;

            static void ThrowKeyframesNotInOrder()
            {
                throw new ArgumentException("Keyframes must be sorted by time in ascending order.");
            }
        }

        /// <summary>
        /// Evaluates the curve at a given time using the specified interpolation mode.
        /// </summary>
        /// <param name="time">The time at which to evaluate the curve.</param>
        /// <returns>The interpolated value at the given time.</returns>
        public Fixed64 Evaluate(Fixed64 time)
        {
            if (Keyframes.Length == 0) return Fixed64.One;

            // Clamp input within the keyframe range
            if (time <= Keyframes[0].Time) return Keyframes[0].Value;
            if (time >= Keyframes[^1].Time) return Keyframes[^1].Value;

            // Find the surrounding keyframes
            for (int i = 0; i < Keyframes.Length - 1; i++)
            {
                if (time >= Keyframes[i].Time && time < Keyframes[i + 1].Time)
                {
                    // Compute interpolation factor
                    Fixed64 t = (time - Keyframes[i].Time) / (Keyframes[i + 1].Time - Keyframes[i].Time);

                    // Choose interpolation method
                    return Mode switch
                    {
                        FixedCurveMode.Step => Keyframes[i].Value,// Immediate transition
                        FixedCurveMode.Smooth => FixedMath.SmoothStep(Keyframes[i].Value, Keyframes[i + 1].Value, t),
                        FixedCurveMode.Cubic => FixedMath.CubicInterpolate(
                                                        Keyframes[i].Value, Keyframes[i + 1].Value,
                                                        Keyframes[i].OutTangent, Keyframes[i + 1].InTangent, t),
                        _ => FixedMath.LinearInterpolate(Keyframes[i].Value, Keyframes[i + 1].Value, t),
                    };
                }
            }

            return Fixed64.One; // Fallback (should never be hit)
        }

        public bool Equals(RefFixedCurve other)
        {
            return Mode == other.Mode && Keyframes.SequenceEqual(other.Keyframes);
        }

        public override bool Equals(object? obj) => false; // ref struct cannot be boxed

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)Mode;
                foreach (var key in Keyframes)
                    hash = (hash * 31) ^ key.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(RefFixedCurve left, RefFixedCurve right) => left.Equals(right);

        public static bool operator !=(RefFixedCurve left, RefFixedCurve right) => !(left == right);
        
        public static implicit operator RefFixedCurve(FixedCurve curve)
        {
            return new RefFixedCurve(curve.Mode, curve.Keyframes);
        }
        public static implicit operator FixedCurve(RefFixedCurve refCurve)
        {
            return new FixedCurve(refCurve.Mode, refCurve.Keyframes.ToArray());
        }
    }
}