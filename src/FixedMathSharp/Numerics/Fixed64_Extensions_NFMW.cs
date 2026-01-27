namespace FixedMathSharp;

public partial struct Fixed64
{
    private static readonly Fixed64 MachineEpsilonFloat = Fixed64.Epsilon;

    public static Fixed64 MaxValue => new(FixedMath.MAX_VALUE_L);
    public static Fixed64 MinValue => new(FixedMath.MIN_VALUE_L);

    public static Fixed64 Pi => FixedMath.PI;
    public static Fixed64 HalfPi => FixedMath.PiOver2;
    public static Fixed64 TwoPi => FixedMath.TwoPI;
    public static Fixed64 PiOver4 => FixedMath.PiOver4;

    public static Fixed64 DegToRad => FixedMath.Deg2Rad;
    public static Fixed64 RadToDeg => FixedMath.Rad2Deg;

    public static bool operator ==(Fixed64 f1, int f2) => f1 == (Fixed64)f2;
    public static bool operator !=(Fixed64 f1, int f2) => f1 != (Fixed64)f2;
    public static bool operator <(Fixed64 f1, int f2) => f1 < (Fixed64)f2;
    public static bool operator >(Fixed64 f1, int f2) => f1 > (Fixed64)f2;
    public static bool operator <=(Fixed64 f1, int f2) => f1 <= (Fixed64)f2;
    public static bool operator >=(Fixed64 f1, int f2) => f1 >= (Fixed64)f2;
    public static bool operator ==(int f1, Fixed64 f2) => (Fixed64)f1 == f2;
    public static bool operator !=(int f1, Fixed64 f2) => (Fixed64)f1 != f2;
    public static bool operator <(int f1, Fixed64 f2) => (Fixed64)f1 < f2;
    public static bool operator >(int f1, Fixed64 f2) => (Fixed64)f1 > f2;
    public static bool operator >=(int f1, Fixed64 f2) => (Fixed64)f1 >= f2;
    public static bool operator <=(int f1, Fixed64 f2) => (Fixed64)f1 <= f2;
    public static Fixed64 operator /(int f1, Fixed64 f2) => (Fixed64)f1 / f2;
    public static Fixed64 operator %(Fixed64 f1, int f2) => f1 % (Fixed64)f2;
    public static Fixed64 operator %(int f1, Fixed64 f2) => (Fixed64)f1 % f2;

    public static bool operator <(Fixed64 f1, long f2) => f1 < (Fixed64)f2;
    public static bool operator >(Fixed64 f1, long f2) => f1 > (Fixed64)f2;
    public static bool operator <=(Fixed64 f1, long f2) => f1 <= (Fixed64)f2;
    public static bool operator >=(Fixed64 f1, long f2) => f1 >= (Fixed64)f2;
    public static bool operator <(long f1, Fixed64 f2) => (Fixed64)f1 < f2;
    public static bool operator >(long f1, Fixed64 f2) => (Fixed64)f1 > f2;
    public static bool operator >=(long f1, Fixed64 f2) => (Fixed64)f1 >= f2;
    public static bool operator <=(long f1, Fixed64 f2) => (Fixed64)f1 <= f2;
    public static Fixed64 operator +(Fixed64 f1, long f2) => f1 + (Fixed64)f2;
    public static Fixed64 operator +(long f1, Fixed64 f2) => (Fixed64)f1 + f2;
    public static Fixed64 operator -(Fixed64 f1, long f2) => f1 - (Fixed64)f2;
    public static Fixed64 operator -(long f1, Fixed64 f2) => (Fixed64)f1 - f2;
    public static Fixed64 operator *(Fixed64 f1, long f2) => f1 * (Fixed64)f2;
    public static Fixed64 operator *(long f1, Fixed64 f2) => (Fixed64)f1 * f2;
    public static Fixed64 operator /(Fixed64 f1, long f2) => f1 / (Fixed64)f2;
    public static Fixed64 operator /(long f1, Fixed64 f2) => (Fixed64)f1 / f2;
    public static Fixed64 operator %(Fixed64 f1, long f2) => f1 % (Fixed64)f2;
    public static Fixed64 operator %(long f1, Fixed64 f2) => (Fixed64)f1 % f2;

    public static Fixed64 Sqrt(Fixed64 f) => (FixedMath.Sqrt(f));

    public static Fixed64 Acos(Fixed64 f) => (FixedMath.Acos(f));

    public static Fixed64 Atan2(Fixed64 a, Fixed64 b) => (FixedMath.Atan2(a, b));

    public static Fixed64 Round(Fixed64 f) => (FixedMath.Round(f));

    public static Fixed64 Sin(Fixed64 f) => (FixedMath.Sin(f));

    public static Fixed64 Cos(Fixed64 f) => (FixedMath.Cos(f));

    public static Fixed64 Floor(Fixed64 f) => (FixedMath.Floor(f));

    public static Fixed64 Ceiling(Fixed64 f) => (FixedMath.Ceiling(f));

    public static Fixed64 Hypot(Fixed64 a, Fixed64 b) => Sqrt(a * a + b * b);

    public static Fixed64 Clamp(Fixed64 value, Fixed64 min, Fixed64 max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }
    
    public static Fixed64 Abs(Fixed64 value) => value.Abs();

    public static bool WithinEpsilon(Fixed64 floatA, Fixed64 floatB) => Abs(floatA - floatB) < MachineEpsilonFloat;

    /// <summary>
    /// Linearly interpolates between two values.
    /// </summary>
    /// <param name="value1">Source value.</param>
    /// <param name="value2">Source value.</param>
    /// <param name="amount">
    /// Value between 0 and 1 indicating the weight of value2.
    /// </param>
    /// <returns>Interpolated value.</returns>
    /// <remarks>
    /// This method performs the linear interpolation based on the following formula.
    /// <c>value1 + (value2 - value1) * amount</c>
    /// Passing amount a value of 0 will cause value1 to be returned, a value of 1 will
    /// cause value2 to be returned.
    /// </remarks>
    public static Fixed64 Lerp(Fixed64 value1, Fixed64 value2, Fixed64 amount)
    {
        return value1 + (value2 - value1) * amount;
    }

    /// <summary>
    /// Returns the Cartesian coordinate for one axis of a point that is defined by a
    /// given triangle and two normalized barycentric (areal) coordinates.
    /// </summary>
    /// <param name="value1">
    /// The coordinate on one axis of vertex 1 of the defining triangle.
    /// </param>
    /// <param name="value2">
    /// The coordinate on the same axis of vertex 2 of the defining triangle.
    /// </param>
    /// <param name="value3">
    /// The coordinate on the same axis of vertex 3 of the defining triangle.
    /// </param>
    /// <param name="amount1">
    /// The normalized barycentric (areal) coordinate b2, equal to the weighting factor
    /// for vertex 2, the coordinate of which is specified in value2.
    /// </param>
    /// <param name="amount2">
    /// The normalized barycentric (areal) coordinate b3, equal to the weighting factor
    /// for vertex 3, the coordinate of which is specified in value3.
    /// </param>
    /// <returns>
    /// Cartesian coordinate of the specified point with respect to the axis being used.
    /// </returns>
    public static Fixed64 Barycentric(
        Fixed64 value1,
        Fixed64 value2,
        Fixed64 value3,
        Fixed64 amount1,
        Fixed64 amount2
    )
    {
        return value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;
    }

    /// <summary>
    /// Performs a Catmull-Rom interpolation using the specified positions.
    /// </summary>
    /// <param name="value1">The first position in the interpolation.</param>
    /// <param name="value2">The second position in the interpolation.</param>
    /// <param name="value3">The third position in the interpolation.</param>
    /// <param name="value4">The fourth position in the interpolation.</param>
    /// <param name="amount">Weighting factor.</param>
    /// <returns>A position that is the result of the Catmull-Rom interpolation.</returns>
    public static Fixed64 CatmullRom(
        Fixed64 value1,
        Fixed64 value2,
        Fixed64 value3,
        Fixed64 value4,
        Fixed64 amount
    )
    {
        /* Using formula from http://www.mvps.org/directx/articles/catmull/
         * Internally using doubles not to lose precision.
         */
        Fixed64 amountSquared = amount * amount;
        Fixed64 amountCubed = amountSquared * amount;
        return (Fixed64)(
            (Fixed64)0.5f *
            (
                (((Fixed64)2.0f * value2 + (value3 - value1) * amount) +
                 (((Fixed64)2.0f * value1 - (Fixed64)5.0f * value2 + (Fixed64)4.0f * value3 - value4) *
                  amountSquared) +
                 ((Fixed64)3.0f * value2 - value1 - (Fixed64)3.0f * value3 + value4) * amountCubed)
            )
        );
    }

    public static Fixed64 Atan(Fixed64 value)
    {
        return FixedMath.Atan(value);
    }
    
    public static Fixed64 Max(Fixed64 a, Fixed64 b)
    {
        return a > b ? a : b;
    }
    
    public static Fixed64 Min(Fixed64 a, Fixed64 b)
    {
        return a < b ? a : b;
    }
}