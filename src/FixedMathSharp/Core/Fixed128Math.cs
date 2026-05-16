using System;
using System.Runtime.CompilerServices;

namespace FixedMathSharp
{
    /// <summary>
    /// A static class that provides a variety of fixed-point math functions.
    /// Fixed-point numbers are represented as <see cref="Fixed128"/>.
    /// </summary>
    public static class Fixed128Math
    {
        #region Fields and Constants

        /// <summary>
        /// Represents the number of bits to shift for fixed-point representation.
        /// </summary>
        public const int SHIFT_AMOUNT_I = 64;
        /// <summary>
        /// Represents the maximum value that can be produced by left-shifting 1 by SHIFT_AMOUNT_I bits and subtracting 1.
        /// </summary>
        /// <remarks>
        /// This constant is typically used as a bitmask to extract or limit values to the range
        /// defined by SHIFT_AMOUNT_I. 
        /// The value is always non-negative and fits within a 32-bit unsigned
        /// integer.
        /// </remarks>
        public static readonly ulong MAX_SHIFTED_AMOUNT_UI = (ulong)((Int128.One << SHIFT_AMOUNT_I) - Int128.One);
        /// <summary>
        /// Represents a bitmask with all bits set except for the lowest SHIFT_AMOUNT_I bits.
        /// </summary>
        /// <remarks>
        /// This constant is typically used to isolate or clear the lower SHIFT_AMOUNT_I bits of
        /// an unsigned 64-bit value. 
        /// The value of SHIFT_AMOUNT_I determines how many least significant bits are masked out.
        /// </remarks>
        public static readonly UInt128 MASK_UL = UInt128.MaxValue << SHIFT_AMOUNT_I;

        /// <summary>
        /// Represents the largest possible value for a 64-bit fixed-point number.
        /// </summary>
        /// <remarks>
        /// Use this constant to perform comparisons or to initialize variables that require the
        /// maximum representable value for a 64-bit fixed-point type.
        /// </remarks>
        public static readonly Int128 MAX_VALUE_L = Int128.MaxValue;
        /// <summary>
        /// Represents the smallest possible value for a 64-bit fixed-point number.
        /// </summary>
        /// <remarks>
        /// Use this constant to check for underflow conditions or to initialize variables that
        /// require the minimum representable value for a 64-bit fixed-point type.
        /// </remarks>
        public static readonly Int128 MIN_VALUE_L = Int128.MinValue;

        /// <summary>
        /// Represents the value 1 shifted left by the number of bits specified by SHIFT_AMOUNT_I.
        /// </summary>
        public static readonly Int128 ONE_L = Int128.One << SHIFT_AMOUNT_I;

        // Precomputed scale factors only for performance-critical scenarios to avoid division at runtime

        /// <summary>
        /// Represents the precomputed scale factor used for floating-point calculations.
        /// </summary>
        /// <remarks>
        /// This constant is intended only for converting fixed-point values to floating-point representations in performance-critical scenarios.
        /// </remarks>
        public static readonly float SCALE_FACTOR_F = 1.0f / (float)ONE_L;
        /// <summary>
        /// Represents the precomputed scale factor used for double-precision calculations.
        /// </summary>
        /// <remarks>
        /// This constant is intended only for converting fixed-point values to double-precision representations in performance-critical scenarios.
        /// </remarks>
        public static readonly double SCALE_FACTOR_D = 1.0 / (double)ONE_L;
        /// <summary>
        /// Represents the precomputed scale factor used for decimal calculations.
        /// </summary>
        /// <remarks>
        /// This constant is intended only for converting fixed-point values to decimal representations in performance-critical scenarios.
        /// </remarks>
        public static readonly decimal SCALE_FACTOR_M = 1.0m / (decimal)ONE_L;

        /// <summary>
        /// The smallest non-zero raw increment representable by Fixed128.
        /// </summary>
        public const long MIN_INCREMENT_L = 1L;

        /// <summary>
        /// Default tolerance for fuzzy comparisons.
        /// Approximately 2^-48 (3.5527137e-15) in value space.
        /// </summary>
        public static readonly Int128 DEFAULT_TOLERANCE_L = Int128.One << (SHIFT_AMOUNT_I - 48);

        /// <summary>
        /// Represents the smallest possible value that can be represented by the Fixed128 format.
        /// </summary>
        /// <remarks>
        /// Precision of this type is 2^-SHIFT_AMOUNT, 
        /// i.e. 1 / (2^SHIFT_AMOUNT) where SHIFT_AMOUNT defines the fractional bits.
        /// </remarks>
        public const long PRECISION_L = MIN_INCREMENT_L;

        /// <summary>
        ///  The smallest value that a Fixed128 can have different from zero.
        /// </summary>
        /// <remarks>
        /// With the following rules:
        ///      anyValue + Epsilon = anyValue
        ///      anyValue - Epsilon = anyValue
        ///      0 + Epsilon = Epsilon
        ///      0 - Epsilon = -Epsilon
        ///  A value Between any number and Epsilon will result in an arbitrary number due to truncating errors.
        /// </remarks>
        public static readonly Int128 EPSILON_L = Int128.One << (SHIFT_AMOUNT_I - 40);

        #endregion

        #region FixedMath Operations

        /// <summary>
        /// Produces a value with the magnitude of the first argument and the sign of the second argument.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed128 CopySign(Fixed128 x, Fixed128 y)
        {
            return y >= Fixed128.Zero ? x.Abs() : -x.Abs();
        }

        /// <summary>
        /// Clamps value between 0 and 1 and returns value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed128 Clamp01(Fixed128 value)
        {
            return value < Fixed128.Zero ? Fixed128.Zero : value > Fixed128.One ? Fixed128.One : value;
        }

        /// <summary>
        /// Clamps a fixed-point value between the given minimum and maximum values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed128 Clamp(Fixed128 f1, Fixed128 min, Fixed128 max)
        {
            return f1 < min ? min : f1 > max ? max : f1;
        }

        /// <summary>
        /// Clamps a value to the inclusive range [min, max].
        /// </summary>
        /// <typeparam name="T">The type of the value, must implement IComparable&lt;T&gt;.</typeparam>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <returns>The clamped value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(max) > 0) return max;
            if (value.CompareTo(min) < 0) return min;
            return value;
        }

        /// <summary>
        /// Clamps the value between -1 and 1 inclusive.
        /// </summary>
        /// <param name="f1">The Fixed128 value to clamp.</param>
        /// <returns>Returns a value clamped between -1 and 1.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed128 ClampOne(Fixed128 f1)
        {
            return f1 > Fixed128.One ? Fixed128.One : f1 < -Fixed128.One ? -Fixed128.One : f1;
        }

        /// <summary>
        /// Returns the absolute value of a Fixed128 number.
        /// </summary>
        public static Fixed128 Abs(Fixed128 value)
        {
            // For the minimum value, return the max to avoid overflow
            if (value.rawValue == MIN_VALUE_L)
                return new Fixed128(MAX_VALUE_L);

            // Use branchless absolute value calculation
            Int128 mask = value.rawValue >> 127; // If negative, mask will be all 1s; if positive, all 0s
            return Fixed128.FromRaw((value.rawValue + mask) ^ mask);
        }

        /// <summary>
        /// Returns the smallest integral value that is greater than or equal to the specified number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed128 Ceiling(Fixed128 value)
        {
            bool hasFractionalPart = (value.rawValue & MAX_SHIFTED_AMOUNT_UI) != 0;
            return hasFractionalPart ? value.Floor() + Fixed128.One : value;
        }

        /// <summary>
        /// Returns the largest integer less than or equal to the specified number (floor function).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed128 Floor(Fixed128 value)
        {
            // Efficiently zeroes out the fractional part
            return Fixed128.FromRaw((long)((ulong)value.rawValue & FixedMath.MASK_UL));
        }

        /// <summary>
        /// Returns the larger of two fixed-point values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed128 Max(Fixed128 f1, Fixed128 f2)
        {
            return f1 >= f2 ? f1 : f2;
        }

        /// <summary>
        /// Returns the smaller of two fixed-point values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed128 Min(Fixed128 a, Fixed128 b)
        {
            return (a < b) ? a : b;
        }

        /// <summary>
        /// Rounds a fixed-point number to the nearest integral value, based on the specified rounding mode.
        /// </summary>
        public static Fixed128 Round(Fixed128 value, MidpointRounding mode = MidpointRounding.ToEven)
        {
            Int128 fractionalPart = value.rawValue & MAX_SHIFTED_AMOUNT_UI;
            Fixed128 integralPart = value.Floor();
            if (fractionalPart < Fixed128.Half.rawValue)
                return integralPart;

            if (fractionalPart > Fixed128.Half.rawValue)
                return integralPart + Fixed128.One;

            // When value is exactly Fixed128.Halfway between two numbers
            return mode switch
            {
                MidpointRounding.AwayFromZero => value.rawValue > 0 ? integralPart + Fixed128.One : integralPart,// For negative midpoints, Floor() is already away from zero
                _ => (integralPart.rawValue & ONE_L) == 0 ? integralPart : integralPart + Fixed128.One,// Rounds to the nearest even number (default behavior)
            };
        }

        public static ReadOnlySpan<long> Pow10Lookup => [
            1,           // 10^0 = 1
            10,          // 10^1 = 10
            100,         // 10^2 = 100
            1000,        // 10^3 = 1000
            10000,       // 10^4 = 10000
            100000,      // 10^5 = 100000
            1000000,     // 10^6 = 1000000
            10000000,    // 10^7 = 1000000
            100000000,   // 10^8 = 1000000
            1000000000,  // 10^9 = 1000000
            10000000000, // 10^10 = 1000000
            100000000000, // 10^11 = 1000000
            1000000000000, // 10^12 = 1000000
            10000000000000, // 10^13 = 1000000
            100000000000000, // 10^14 = 1000000
            1000000000000000, // 10^15 = 1000000
            10000000000000000, // 10^16 = 1000000
            100000000000000000, // 10^17 = 1000000
            1000000000000000000, // 10^18 = 1000000
        ];

        /// <summary>
        /// Rounds a fixed-point number to a specific number of decimal places.
        /// </summary>
        public static Fixed128 RoundToPrecision(Fixed128 value, int decimalPlaces, MidpointRounding mode = MidpointRounding.ToEven)
        {
            if (decimalPlaces < 0 || decimalPlaces >= Pow10Lookup.Length)
                throw new ArgumentOutOfRangeException(nameof(decimalPlaces), "Decimal places out of range.");

            var factor = Pow10Lookup[decimalPlaces];
            Fixed128 scaled = value * factor;
            Int128 rounded = Round(scaled, mode).rawValue;
            return new Fixed128(rounded + (factor / 2)) / factor;
        }

        /// <summary>
        /// Squares the Fixed128 value.
        /// </summary>
        /// <param name="value">The Fixed128 value to square.</param>
        /// <returns>The squared value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed128 Squared(Fixed128 value)
        {
            return value * value;
        }

        /// <summary>
        /// Adds two fixed-point numbers without performing overflow checking.
        /// </summary>  
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed128 FastAdd(Fixed128 x, Fixed128 y)
        {
            return Fixed128.FromRaw(x.rawValue + y.rawValue);
        }

        /// <summary>
        /// Subtracts two fixed-point numbers without performing overflow checking.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed128 FastSub(Fixed128 x, Fixed128 y)
        {
            return Fixed128.FromRaw(x.rawValue - y.rawValue);
        }

        /// <summary>
        /// Multiplies two fixed-point numbers without overflow checking for performance-critical scenarios.
        /// </summary>
        public static Fixed128 FastMul(Fixed128 x, Fixed128 y)
        {
            var xl = x.rawValue;
            var yl = y.rawValue;

            // Split values into high and low bits for long multiplication
            UInt128 xlo = (UInt128)(xl & MAX_SHIFTED_AMOUNT_UI);
            Int128 xhi = xl >> SHIFT_AMOUNT_I;
            UInt128 ylo = (UInt128)(yl & MAX_SHIFTED_AMOUNT_UI);
            Int128 yhi = yl >> SHIFT_AMOUNT_I;

            // Perform partial products
            UInt128 lolo = xlo * ylo;
            Int128 lohi = (Int128)xlo * yhi;
            Int128 hilo = xhi * (Int128)ylo;
            Int128 hihi = xhi * yhi;

            // Combine the results
            UInt128 loResult = lolo >> SHIFT_AMOUNT_I;
            Int128 midResult1 = lohi;
            Int128 midResult2 = hilo;
            Int128 hiResult = hihi << SHIFT_AMOUNT_I;

            Int128 sum = (Int128)loResult + midResult1 + midResult2 + hiResult;
            return Fixed128.FromRaw(sum);
        }

        /// <summary>
        /// Fast modulus without the checks performed by the '%' operator.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Fixed128 FastMod(Fixed128 x, Fixed128 y)
        {
            return Fixed128.FromRaw(x.rawValue % y.rawValue);
        }

        /// <summary>
        /// Performs a smooth step interpolation between two values.
        /// </summary>
        /// <remarks>
        /// The interpolation follows a cubic Hermite curve where the function starts at `a`,
        /// accelerates, and then decelerates towards `b`, ensuring smooth transitions.
        /// </remarks>
        /// <param name="a">The starting value.</param>
        /// <param name="b">The ending value.</param>
        /// <param name="t">A value between 0 and 1 that represents the interpolation factor.</param>
        /// <returns>The interpolated value between `a` and `b`.</returns>
        public static Fixed128 SmoothStep(Fixed128 a, Fixed128 b, Fixed128 t)
        {
            t = t * t * (Fixed128.Three - Fixed128.Two * t);
            return LinearInterpolate(a, b, t);
        }

        /// <summary>
        /// Performs a cubic Hermite interpolation between two points, using specified tangents.
        /// </summary>
        /// <remarks>
        /// This method interpolates smoothly between `p0` and `p1` while considering the tangents `m0` and `m1`.
        /// It is useful for animation curves and smooth motion transitions.
        /// </remarks>
        /// <param name="p0">The first point.</param>
        /// <param name="p1">The second point.</param>
        /// <param name="m0">The tangent (slope) at `p0`.</param>
        /// <param name="m1">The tangent (slope) at `p1`.</param>
        /// <param name="t">A value between 0 and 1 that represents the interpolation factor.</param>
        /// <returns>The interpolated value between `p0` and `p1`.</returns>
        public static Fixed128 CubicInterpolate(Fixed128 p0, Fixed128 p1, Fixed128 m0, Fixed128 m1, Fixed128 t)
        {
            Fixed128 t2 = t * t;
            Fixed128 t3 = t2 * t;
            return (Fixed128.Two * p0 - Fixed128.Two * p1 + m0 + m1) * t3
                 + (-Fixed128.Three * p0 + Fixed128.Three * p1 - Fixed128.Two * m0 - m1) * t2
                 + m0 * t + p0;
        }

        /// <summary>
        /// Performs linear interpolation between two fixed-point values based on the interpolant t (0 greater or equal to `t` and less than or equal to 1).
        /// </summary>
        public static Fixed128 LinearInterpolate(Fixed128 from, Fixed128 to, Fixed128 t)
        {
            if (t.rawValue >= ONE_L)
                return to;
            if (t.rawValue <= 0)
                return from;

            return (to * t) + (from * (Fixed128.One - t));
        }

        /// <summary>
        /// Moves a value from 'from' to 'to' by a maximum step of 'maxAmount'. 
        /// Ensures the value does not exceed 'to'.
        /// </summary>
        public static Fixed128 MoveTowards(Fixed128 from, Fixed128 to, Fixed128 maxAmount)
        {
            if (from < to)
            {
                from += maxAmount;
                if (from > to)
                    from = to;
            }
            else if (from > to)
            {
                from -= maxAmount;
                if (from < to)
                    from = to;
            }

            return Fixed128.FromRaw(from.rawValue);
        }

        /// <summary>
        /// Adds two <see cref="long"/> values and checks for overflow.
        /// If an overflow occurs during addition, the <paramref name="overflow"/> parameter is set to true.
        /// </summary>
        /// <param name="x">The first operand to add.</param>
        /// <param name="y">The second operand to add.</param>
        /// <param name="overflow">
        /// A reference parameter that is set to true if an overflow is detected during the addition.
        /// The existing value of <paramref name="overflow"/> is preserved if already true.
        /// </param>
        /// <returns>The sum of <paramref name="x"/> and <paramref name="y"/>.</returns>
        /// <remarks>
        /// Overflow is detected by checking for a change in the sign bit that indicates a wrap-around.
        /// Additionally, a special check is performed for adding <see cref="Fixed128.MIN_VALUE"/> and -1, 
        /// as this is a known edge case for overflow.
        /// </remarks>
        public static long AddOverflowHelper(long x, long y, ref bool overflow)
        {
            long sum = x + y;
            // Check for overflow using sign bit changes
            overflow |= ((x ^ y ^ sum) & MIN_VALUE_L) != 0;
            // Special check for the case when x is long.Fixed128.MinValue and y is negative
            if (x == long.MinValue && y == -1)
                overflow = true;
            return sum;
        }

        #endregion
    }
}
