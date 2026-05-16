using System;
using System.Runtime.CompilerServices;


namespace FixedMathSharp;

/// <summary>
/// Provides extension methods for the Fixed128 type, enabling additional mathematical, conversion, and comparison operations using a fluent syntax.
/// </summary>
public static class Fixed128Extensions
{
    #region Fixed128 Operations

    /// <inheritdoc cref="Fixed128.Sign(Fixed128)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Sign(this Fixed128 value)
    {
        return Fixed128.Sign(value);
    }

    /// <inheritdoc cref="Fixed128.IsInteger(Fixed128)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInteger(this Fixed128 value)
    {
        return Fixed128.IsInteger(value);
    }

    /// <inheritdoc cref="Fixed128Math.Squared(Fixed128)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 Squared(this Fixed128 value)
    {
        return Fixed128Math.Squared(value);
    }

    /// <inheritdoc cref="Fixed128Math.Round(Fixed128, MidpointRounding)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 Round(this Fixed128 value, MidpointRounding mode = MidpointRounding.ToEven)
    {
        return Fixed128Math.Round(value, mode);
    }

    /// <inheritdoc cref="Fixed128Math.RoundToPrecision(Fixed128, int, MidpointRounding)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 RoundToPrecision(this Fixed128 value, int places, MidpointRounding mode = MidpointRounding.ToEven)
    {
        return Fixed128Math.RoundToPrecision(value, places, mode);
    }

    /// <inheritdoc cref="Fixed128Math.ClampOne(Fixed128)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 ClampOne(this Fixed128 f1)
    {
        return Fixed128Math.ClampOne(f1);
    }

    /// <inheritdoc cref="Fixed128Math.Clamp01(Fixed128)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 Clamp01(this Fixed128 f1)
    {
        return Fixed128Math.Clamp01(f1);
    }

    /// <inheritdoc cref="Fixed128Math.Abs(Fixed128)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 Abs(this Fixed128 value)
    {
        return Fixed128Math.Abs(value);
    }

    /// <summary>
    /// Checks if the absolute value of x is less than y.
    /// </summary>
    /// <param name="x">The value to compare.</param>
    /// <param name="y">The comparison threshold.</param>
    /// <returns>True if |x| &lt; y; otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AbsLessThan(this Fixed128 x, Fixed128 y)
    {
        return Abs(x) < y;
    }

    /// <inheritdoc cref="Fixed128Math.FastAdd(Fixed128, Fixed128)" />
    public static Fixed128 FastAdd(this Fixed128 a, Fixed128 b)
    {
        return Fixed128Math.FastAdd(a, b);
    }

    /// <inheritdoc cref="Fixed128Math.FastSub(Fixed128, Fixed128)" />
    public static Fixed128 FastSub(this Fixed128 a, Fixed128 b)
    {
        return Fixed128Math.FastSub(a, b);
    }

    /// <inheritdoc cref="Fixed128Math.FastMul(Fixed128, Fixed128)" />
    public static Fixed128 FastMul(this Fixed128 a, Fixed128 b)
    {
        return Fixed128Math.FastMul(a, b);
    }

    /// <inheritdoc cref="Fixed128Math.FastMod(Fixed128, Fixed128)" />
    public static Fixed128 FastMod(this Fixed128 a, Fixed128 b)
    {
        return Fixed128Math.FastMod(a, b);
    }

    /// <inheritdoc cref="Fixed128Math.Floor(Fixed128)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 Floor(this Fixed128 value)
    {
        return Fixed128Math.Floor(value);
    }

    /// <inheritdoc cref="Fixed128Math.Ceiling(Fixed128)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 Ceiling(this Fixed128 value)
    {
        return Fixed128Math.Ceiling(value);
    }

    /// <summary>
    /// Rounds the Fixed128 value to the nearest integer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int RoundToInt(this Fixed128 x)
    {
        return Fixed128.RawToInt(Round(x));
    }

    /// <summary>
    /// Rounds up the Fixed128 value to the nearest integer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CeilToInt(this Fixed128 x)
    {
        return Fixed128.RawToInt(Ceiling(x));
    }

    /// <summary>
    /// Rounds down the Fixed128 value to the nearest integer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int FloorToInt(this Fixed128 x)
    {
        return Fixed128.RawToInt(Floor(x));
    }

    #endregion

    #region Conversion

    /// <summary>
    /// Converts the Fixed128 value to a string formatted to 2 decimal places.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToFormattedString(this Fixed128 f1)
    {
        return f1.ToPreciseFloat().ToString("0.##");
    }

    /// <summary>
    /// Converts the Fixed128 value to a double with specified decimal precision.
    /// </summary>
    /// <param name="f1">The Fixed128 value to convert.</param>
    /// <param name="precision">The number of decimal places to round to.</param>
    /// <returns>The formatted double value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ToFormattedDouble(this Fixed128 f1, int precision = 2)
    {
        return Math.Round((double)f1, precision, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// Converts the Fixed128 value to a float with 2 decimal points of precision.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ToFormattedFloat(this Fixed128 f1)
    {
        return (float)ToFormattedDouble(f1);
    }

    /// <summary>
    /// Converts the Fixed128 value to a precise float representation (without rounding).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ToPreciseFloat(this Fixed128 f1)
    {
        return (float)(double)f1;
    }

    // /// <summary>
    // /// Converts the angle in degrees to radians.
    // /// </summary>
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public static Fixed128 ToRadians(this Fixed128 angleInDegrees)
    // {
    //     return Fixed128Math.DegToRad(angleInDegrees);
    // }
    //
    // /// <summary>
    // /// Converts the angle in radians to degree.
    // /// </summary>
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public static Fixed128 ToDegree(this Fixed128 angleInRadians)
    // {
    //     return Fixed128Math.RadToDeg(angleInRadians);
    // }

    #endregion

    #region Equality

    /// <summary>
    /// Checks if the value is greater than epsilon (positive or negative).
    /// Useful for determining if a value is effectively non-zero with a given precision.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool MoreThanEpsilon(this Fixed128 d)
    {
        return d.Abs() > Fixed128.Epsilon;
    }

    /// <summary>
    /// Checks if the value is less than epsilon (i.e., effectively zero).
    /// Useful for determining if a value is close enough to zero with a given precision.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThanEpsilon(this Fixed128 d)
    {
        return d.Abs() < Fixed128.Epsilon;
    }

    /// <summary>
    /// Helper method to compare individual vector components for approximate equality, allowing a fractional difference.
    /// Handles zero components by only using the allowed percentage difference.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool FuzzyComponentEqual(this Fixed128 a, Fixed128 b, Fixed128 percentage)
    {
        var diff = (a - b).Abs();
        var allowedErr = a.Abs() * percentage;
        // Compare directly to percentage if a is zero
        // Otherwise, use percentage of a's magnitude
        return a.LessThanEpsilon() ? diff <= percentage : diff <= allowedErr;
    }

    #endregion
}