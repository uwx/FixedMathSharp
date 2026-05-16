using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MemoryPack;
using MessagePack;

namespace FixedMathSharp;

/// <summary>
/// Represents a Q(128-SHIFT_AMOUNT).SHIFT_AMOUNT fixed-point number.
/// Provides high precision for fixed-point arithmetic where SHIFT_AMOUNT bits 
/// are used for the fractional part and (64 - SHIFT_AMOUNT) bits for the integer part.
/// The precision is determined by SHIFT_AMOUNT, which defines the resolution of fractional values.
/// </summary>
[Serializable]
[MemoryPackable]
[MessagePackObject]
public readonly partial struct Fixed128 : IEquatable<Fixed128>, IComparable<Fixed128>, IEqualityComparer<Fixed128>
{
    #region Static Readonly Fields

    /// <inheritdoc cref="FixedMathSharp.Fixed128Math.MAX_VALUE_L" />
    public static readonly Fixed128 MAX_VALUE = new(Fixed128Math.MAX_VALUE_L);
    /// <inheritdoc cref="Fixed128Math.MIN_VALUE_L" />
    public static readonly Fixed128 MIN_VALUE = new(Fixed128Math.MIN_VALUE_L);

    /// <inheritdoc cref="Fixed128Math.ONE_L" />
    public static readonly Fixed128 One = new(Fixed128Math.ONE_L);
    /// <summary>
    /// Represents the value 2 shifted left by the number of bits specified by SHIFT_AMOUNT_I.
    /// </summary>
    public static readonly Fixed128 Two = One * 2;
    /// <summary>
    /// Represents the value 3 shifted left by the number of bits specified by SHIFT_AMOUNT_I.
    /// </summary>
    public static readonly Fixed128 Three = One * 3;
    /// <summary>
    /// Represents the value 0.5 shifted left by the number of bits specified by SHIFT_AMOUNT_I.
    /// </summary>
    public static readonly Fixed128 Half = One / 2;
    /// <summary>
    /// Represents the value 0.25 shifted left by the number of bits specified by SHIFT_AMOUNT_I.
    /// </summary>
    public static readonly Fixed128 Quarter = One / 4;
    /// <summary>
    /// Represents the value 0.125 shifted left by the number of bits specified by SHIFT_AMOUNT_I.
    /// </summary>
    public static readonly Fixed128 Eighth = One / 8;
    /// <summary>
    /// Represents the value 0 as a fixed-point number.
    /// </summary>
    public static readonly Fixed128 Zero = new(0);


    /// <inheritdoc cref="Fixed128Math.MIN_INCREMENT_L" />
    public static readonly Fixed128 MinIncrement = new(Fixed128Math.MIN_INCREMENT_L);

    /// <inheritdoc cref="Fixed128Math.DEFAULT_TOLERANCE_L" />
    public static readonly Fixed128 Epsilon = new(Fixed128Math.DEFAULT_TOLERANCE_L);

    #endregion

    #region Fields

    /// <summary>
    /// The underlying raw Int128 value representing the fixed-point number.
    /// </summary>
    [Key(0)]
    [JsonInclude]
    [MemoryPackInclude]
    public readonly Int128 rawValue;

    public static Fixed128 MinusOne => new(-Fixed128Math.ONE_L);

    /// <inheritdoc cref="Fixed128Math.PRECISION_L" />
    public static Fixed128 Precision => new(Fixed128Math.PRECISION_L);

    #endregion

    #region Constructors

    /// <summary>
    /// Internal constructor for a Fixed128 from a raw Int128 value.
    /// </summary>
    /// <param name="rawValue">Raw Int128 value representing the fixed-point number.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining), JsonConstructor]
    internal Fixed128(Int128 rawValue) => this.rawValue = rawValue;

    /// <summary>
    /// Constructs a Fixed128 from an integer, with the fractional part set to zero.
    /// </summary>
    /// <param name="value">Integer value to convert to </param>
    public Fixed128(int value) : this((Int128)value << Fixed128Math.SHIFT_AMOUNT_I) { }

    /// <summary>
    /// Constructs a Fixed128 from a double-precision floating-point value.
    /// </summary>
    /// <remarks>
    /// The value is multiplied by the scaling factor (2^SHIFT_AMOUNT) and 
    /// rounded to the nearest integer to fit into the fixed-point representation.
    /// </remarks>
    /// <param name="value">Double value to convert to </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 CreateFromDouble(double value) => new((Int128)Math.Round(value * (double)Fixed128Math.ONE_L));

    #endregion

    #region Methods (Instance)

    /// <summary>
    /// Offsets the current Fixed128 by an integer value.
    /// </summary>
    /// <param name="x">The integer value to add.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Fixed128 Offset(int x)
    {
        return new Fixed128(rawValue + ((Int128)x << Fixed128Math.SHIFT_AMOUNT_I));
    }

    /// <summary>
    /// Returns the raw value as a string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string RawToString()
    {
        return rawValue.ToString();
    }

    #endregion

    #region Fixed128 Operations

    /// <summary>
    /// Creates a Fixed128 from a fractional number.
    /// </summary>
    /// <param name="numerator">The numerator of the fraction.</param>
    /// <param name="denominator">The denominator of the fraction.</param>
    /// <returns>A Fixed128 representing the fraction.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 Fraction(Fixed128 numerator, Fixed128 denominator)
    {
        return numerator / denominator;
    }

    /// <summary>
    /// Returns a number indicating the sign of a Fix64 number.
    /// Returns 1 if the value is positive, 0 if is 0, and -1 if it is negative.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Sign(Fixed128 value)
    {
        // Return the sign of the value, optimizing for branchless comparison
        return value.rawValue < 0 ? -1 : (value.rawValue > 0 ? 1 : 0);
    }

    /// <summary>
    /// Returns true if the number has no decimal part (i.e., if the number is equivalent to an integer) and False otherwise. 
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInteger(Fixed128 value)
    {
        return ((UInt128)value.rawValue & Fixed128Math.MAX_SHIFTED_AMOUNT_UI) == 0;
    }

    #endregion

    #region Explicit and Implicit Conversions

    /// <summary>
    /// Converts a Fixed64 to a Fixed128 value using explicit casting.
    /// </summary>
    /// <param name="f64">The Fixed64 to convert to a Fixed128 value.</param>
    public static explicit operator Fixed128(Fixed64 f64)
    {
        return FromRaw((Int128)f64.rawValue << (Fixed128Math.SHIFT_AMOUNT_I - FixedMath.SHIFT_AMOUNT_I));
    }
    
    /// <summary>
    /// Truncates a Fixed128 to a Fixed64 value using explicit casting.
    /// </summary>
    /// <param name="f128">The Fixed128 to convert to a Fixed64 value.</param>
    public static explicit operator Fixed64(Fixed128 f128)
    {
        return new Fixed64((long)(f128.rawValue >> (Fixed128Math.SHIFT_AMOUNT_I - FixedMath.SHIFT_AMOUNT_I)));
    }
    
    /// <summary>
    /// Converts a 64-bit signed integer to a Fixed128 value using explicit casting.
    /// </summary>
    /// <remarks>
    /// The conversion interprets the input value as the integer part of the fixed-point number. 
    /// Use this operator when an explicit conversion from Int128 to Fixed128 is required.
    /// </remarks>
    /// <param name="value">The 64-bit signed integer to convert to a Fixed128 value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Fixed128(Int128 value)
    {
        return FromRaw(value << Fixed128Math.SHIFT_AMOUNT_I);
    }

    /// <summary>
    /// Converts a Fixed128 value to a 64-bit signed integer by discarding the fractional part.
    /// </summary>
    /// <remarks>
    /// The conversion truncates any fractional component. 
    /// The result represents the integer portion of the Fixed128 value.</remarks>
    /// <param name="value">The Fixed128 value to convert to a Int128 integer.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Int128(Fixed128 value)
    {
        return value.rawValue >> Fixed128Math.SHIFT_AMOUNT_I;
    }

    /// <summary>
    /// Defines an explicit conversion from a 32-bit signed integer to a Fixed128 value.
    /// </summary>
    /// <param name="value">The 32-bit signed integer to convert to a Fixed128 value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Fixed128(int value)
    {
        return new Fixed128(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int RawToInt(Fixed128 value)
    {
        return (int)(value.rawValue >> Fixed128Math.SHIFT_AMOUNT_I);
    }

    /// <summary>
    /// Defines an explicit conversion from a single-precision floating-point value to a Fixed128 instance.
    /// </summary>
    /// <param name="value">The single-precision floating-point value to convert to Fixed128.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator int(Fixed128 value)
    {
        // truncation toward zero. regularly casting Fixed128 to int doesn't do that
        if (value > Fixed128.Zero)
            return value.FloorToInt();
        else
            return value.CeilToInt();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Fixed128(float value)
    {
        return CreateFromDouble((double)value);
    }

    /// <summary>
    /// Converts a Fixed128 value to its equivalent single-precision floating-point representation.
    /// </summary>
    /// <remarks>
    /// This conversion may result in a loss of precision if the Fixed128 value cannot be exactly  represented as a float.
    /// </remarks>
    /// <param name="value">The Fixed128 value to convert to a float.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator float(Fixed128 value)
    {
        return (float)value.rawValue * Fixed128Math.SCALE_FACTOR_F;
    }

    /// <summary>
    /// Defines an explicit conversion from a double-precision floating-point number to a Fixed128 value.
    /// </summary>
    /// <remarks>
    /// This conversion may result in loss of precision if the double value cannot be exactly represented as a Fixed128.
    /// </remarks>
    /// <param name="value">The double-precision floating-point number to convert to a Fixed128 value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Fixed128(double value)
    {
        return CreateFromDouble(value);
    }

    /// <summary>
    /// Converts a Fixed128 value to its equivalent double-precision floating-point representation.
    /// </summary>
    /// <remarks>
    /// This conversion may result in a loss of precision if the Fixed128 value cannot be exactly represented as a double.
    /// </remarks>
    /// <param name="value">The Fixed128 value to convert to a double.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator double(Fixed128 value)
    {
        return (double)value.rawValue * Fixed128Math.SCALE_FACTOR_D;
    }

    /// <summary>
    /// Defines an explicit conversion from a decimal value to a Fixed128 instance.
    /// </summary>
    /// <param name="value">The decimal value to convert to a Fixed128 instance.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Fixed128(decimal value)
    {
        return CreateFromDouble((double)value);
    }

    /// <summary>
    /// Converts a Fixed128 value to its decimal representation.
    /// </summary>
    /// <remarks>
    /// This operator provides an explicit conversion from Fixed128 to decimal, preserving the numeric value as closely as possible. 
    /// Use this conversion when precise decimal arithmetic is required.
    /// </remarks>
    /// <param name="value">The Fixed128 value to convert to decimal.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator decimal(Fixed128 value)
    {
        return (decimal)value.rawValue * Fixed128Math.SCALE_FACTOR_M;
    }

    #endregion

    #region Arithmetic Operators

    /// <summary>
    /// Adds two Fixed128 numbers, with saturating behavior in case of overflow.
    /// </summary>
    public static Fixed128 operator +(Fixed128 x, Fixed128 y)
    {
        Int128 xl = x.rawValue;
        Int128 yl = y.rawValue;
        Int128 sum = xl + yl;
        // Check for overflow, if signs of operands are equal and signs of sum and x are different
        if (((~(xl ^ yl) & (xl ^ sum)) & Fixed128Math.MIN_VALUE_L) != 0)
            sum = xl > 0 ? Fixed128Math.MAX_VALUE_L : Fixed128Math.MIN_VALUE_L;
        return new Fixed128(sum);
    }

    /// <summary>
    /// Adds an int to x 
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 operator +(Fixed128 x, int y)
    {
        return x + new Fixed128((Int128)y << Fixed128Math.SHIFT_AMOUNT_I);
    }

    /// <summary>
    /// Adds an Fixed128 to x 
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 operator +(int x, Fixed128 y)
    {
        return y + x;
    }

    /// <summary>
    /// Subtracts one Fixed128 number from another, with saturating behavior in case of overflow.
    /// </summary>
    public static Fixed128 operator -(Fixed128 x, Fixed128 y)
    {
        Int128 xl = x.rawValue;
        Int128 yl = y.rawValue;
        Int128 diff = xl - yl;
        // Check for overflow, if signs of operands are different and signs of sum and x are different
        if ((((xl ^ yl) & (xl ^ diff)) & Fixed128Math.MIN_VALUE_L) != 0)
            diff = xl < 0 ? Fixed128Math.MIN_VALUE_L : Fixed128Math.MAX_VALUE_L;
        return new Fixed128(diff);
    }

    /// <summary>
    /// Subtracts an int from x 
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 operator -(Fixed128 x, int y)
    {
        return x - new Fixed128((Int128)y << Fixed128Math.SHIFT_AMOUNT_I);
    }

    /// <summary>
    /// Subtracts a Fixed128 from x 
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 operator -(int x, Fixed128 y)
    {
        return new Fixed128((Int128)x << Fixed128Math.SHIFT_AMOUNT_I) - y;
    }
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 operator *(Fixed128 a, Fixed128 b)
    {
        // Use Math.BigMul to get 128-bit result as high and low parts
        Int128 high = Int128.BigMul(a.rawValue, b.rawValue, out var low);

        // Check the most significant bit that will be dropped for rounding
        UInt128 roundBit = 1UL << (Fixed128Math.SHIFT_AMOUNT_I - 1);

        // Combine high and low parts, shifting right by SHIFT_AMOUNT_I
        Int128 result = (high << (128 - Fixed128Math.SHIFT_AMOUNT_I)) | (low >>> Fixed128Math.SHIFT_AMOUNT_I);

        // Apply rounding
        if (((UInt128)low & roundBit) != 0) result++;

        // Overflow check: if high bits don't match sign extension, clamp
        Int128 signCheck = high >> (Fixed128Math.SHIFT_AMOUNT_I - 1);
        if (signCheck != 0 && signCheck != -1)
            result = high < 0 ? Int128.MinValue : Int128.MaxValue;

        return new Fixed128(result);
    }

    /// <summary>
    /// Multiplies two Fixed128 numbers using full-width 256-bit intermediate precision
    /// and round-half-to-even semantics on the discarded fractional bits.
    /// </summary>
    public static Fixed128 MulPrecise(Fixed128 x, Fixed128 y)
    {
        Int128 xl = x.rawValue;
        Int128 yl = y.rawValue;

        int shift = Fixed128Math.SHIFT_AMOUNT_I;

        if (shift <= 0 || shift >= 128)
            throw new InvalidOperationException($"SHIFT_AMOUNT_I must be in the range 1..128, but was {shift}.");

        // Determine sign of the final result.
        bool negative = ((xl ^ yl) < 0);

        // Convert to unsigned magnitudes safely, including Int128.MinValue.
        UInt128 ax = AbsToUInt64(xl);
        UInt128 ay = AbsToUInt64(yl);

        // Compute exact 128-bit unsigned product: (hi << 64) | lo
        Multiply128to256(ax, ay, out UInt128 hi, out UInt128 lo);

        // Shift-right with round-half-to-even using the FULL discarded remainder.
        UInt128 magnitude = ShiftRightRoundedToEven(hi, lo, shift, out bool roundedOverflow);

        // If rounding overflowed the shifted magnitude, carry it into saturation handling.
        if (!negative)
        {
            if (roundedOverflow || magnitude > (UInt128)Int128.MaxValue)
                return new Fixed128(Fixed128Math.MAX_VALUE_L);

            return new Fixed128((Int128)magnitude);
        }
        else
        {
            // For negative results, magnitude may be exactly 2^63, which maps to Int128.MinValue.
            var minValueMagnitude = new UInt128(0x8000000000000000UL, 0x0000000000000000UL); // TODO is this flipped

            if (roundedOverflow || magnitude > minValueMagnitude)
                return new Fixed128(Fixed128Math.MIN_VALUE_L);

            if (magnitude == minValueMagnitude)
                return new Fixed128(Fixed128Math.MIN_VALUE_L);

            return new Fixed128(-(Int128)magnitude);
        }
    }

    /// <summary>
    /// Returns the absolute value of a signed 128-bit integer as an unsigned 128-bit magnitude,
    /// safely handling Int128.MinValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static UInt128 AbsToUInt64(Int128 value)
    {
        return value < 0
            ? unchecked((UInt128)(~value + 1))
            : (UInt128)value;
    }

    /// <summary>
    /// Computes the exact unsigned 128-bit product of two 64-bit unsigned integers.
    /// The result is returned as hi:lo, where product = (hi &lt;&lt; 64) | lo.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Multiply128to256(UInt128 a, UInt128 b, out UInt128 hi, out UInt128 lo)
    {
        UInt128 aLo = (ulong)a;
        UInt128 aHi = a >> 64;
        UInt128 bLo = (ulong)b;
        UInt128 bHi = b >> 64;

        UInt128 p0 = aLo * bLo;
        UInt128 p1 = aLo * bHi;
        UInt128 p2 = aHi * bLo;
        UInt128 p3 = aHi * bHi;

        UInt128 middle = (p0 >> 64) + (ulong)p1 + (ulong)p2;

        lo = (p0 & 0xFFFFFFFFFFFFFFFFUL) | (middle << 64);
        hi = p3 + (p1 >> 64) + (p2 >> 64) + (middle >> 64);
    }

    /// <summary>
    /// Shifts the unsigned 128-bit value (hi:lo) right by <paramref name="shift"/> bits,
    /// applying round-half-to-even to the discarded bits.
    /// </summary>
    /// <param name="hi">Upper 64 bits of the 128-bit value.</param>
    /// <param name="lo">Lower 64 bits of the 128-bit value.</param>
    /// <param name="shift">Number of bits to shift right. Must be in the range 1..63.</param>
    /// <param name="overflowed">
    /// True if rounding caused the 64-bit shifted result to overflow.
    /// </param>
    /// <returns>
    /// The rounded 64-bit result of ((hi:lo) >> shift).
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static UInt128 ShiftRightRoundedToEven(UInt128 hi, UInt128 lo, int shift, out bool overflowed)
    {
        // Preconditions: 1 <= shift <= 63

        // Integer part after shifting right by 'shift':
        // result = ((hi << (64 - shift)) | (lo >> shift))
        UInt128 result = (hi << (128 - shift)) | (lo >> shift);

        // Discarded remainder bits are the low 'shift' bits of lo.
        UInt128 remainderMask = (UInt128.One << shift) - UInt128.One;
        UInt128 remainder = lo & remainderMask;

        // Halfway value among the discarded bits.
        UInt128 half = UInt128.One << (shift - 1);

        // Round-half-to-even:
        // - round up if remainder > half
        // - if exactly half, round so final result is even
        bool shouldRoundUp =
            remainder > half ||
            (remainder == half && (result & UInt128.One) != 0);

        overflowed = false;

        if (shouldRoundUp)
        {
            UInt128 incremented = result + UInt128.One;
            overflowed = incremented < result;
            result = incremented;
        }

        return result;
    }

    /// <summary>
    /// Multiplies a Fixed128 by an integer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 operator *(Fixed128 x, int y)
    {
        return new Fixed128(x.rawValue * y);
    }

    /// <summary>
    /// Multiplies a Fixed128 by an integer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 operator *(Fixed128 x, long y)
    {
        return new Fixed128(x.rawValue * y);
    }

    /// <summary>
    /// Multiplies an integer by a Fixed128.
    /// </summary>
    public static Fixed128 operator *(int x, Fixed128 y)
    {
        return new Fixed128(y.rawValue * x);
    }

    /// <summary>
    /// Multiplies an integer by a Fixed128.
    /// </summary>
    public static Fixed128 operator *(long x, Fixed128 y)
    {
        return new Fixed128(y.rawValue * x);
    }

    /// <summary>
    /// Divides one Fixed128 number by another, handling division by zero and overflow.
    /// </summary>
    public static Fixed128 operator /(Fixed128 x, Fixed128 y)
    {
        Int128 xl = x.rawValue;
        Int128 yl = y.rawValue;

        if (yl == 0)
        {
            ThrowDivideByZeroException(x);
            return default;
        }

        UInt128 remainder = (UInt128)(xl < 0 ? -xl : xl);
        UInt128 divider = (UInt128)(yl < 0 ? -yl : yl);
        UInt128 quotient = 0UL;
        int bitPos = Fixed128Math.SHIFT_AMOUNT_I + 1;

        // If the divider is divisible by 2^n, take advantage of it.
        while ((divider & 0xF) == 0 && bitPos >= 4)
        {
            divider >>= 4;
            bitPos -= 4;
        }

        while (remainder != 0 && bitPos >= 0)
        {
            var shift = (int)UInt128.LeadingZeroCount(remainder);
            if (shift > bitPos)
                shift = bitPos;

            remainder <<= shift;
            bitPos -= shift;

            UInt128 div = remainder / divider;
            remainder %= divider;
            quotient += div << bitPos;

            // Detect overflow
            if ((div & ~(new UInt128(0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF) >> bitPos)) != 0)
                return ((xl ^ yl) & Fixed128Math.MIN_VALUE_L) == 0 
                    ? new Fixed128(Fixed128Math.MAX_VALUE_L) 
                    : new Fixed128(Fixed128Math.MIN_VALUE_L);

            remainder <<= 1;
            --bitPos;
        }

        // Rounding logic: "Round half to even" or "Banker's rounding"
        if ((quotient & 0x1) != 0)
            quotient += 1;

        Int128 result = (Int128)(quotient >> 1);
        if (((xl ^ yl) & Fixed128Math.MIN_VALUE_L) != 0)
            result = -result;

        return new Fixed128(result);

        [DoesNotReturn]
        static void ThrowDivideByZeroException(Fixed128 a)
        {
            throw new DivideByZeroException($"Attempted to divide {a} by zero.");
        }
    }

    /// <summary>
    /// Divides a Fixed128 by an integer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 operator /(Fixed128 x, int y)
    {
        return x / new Fixed128((Int128)y << Fixed128Math.SHIFT_AMOUNT_I);
    }

    /// <summary>
    /// Divides an integer by a Fixed128
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 operator /(int y, Fixed128 x)
    {
        return new Fixed128((Int128)y << Fixed128Math.SHIFT_AMOUNT_I) / x;
    }

    /// <summary>
    /// Divides a Fixed128 by an integer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 operator /(Fixed128 x, long y)
    {
        return x / new Fixed128((Int128)y << Fixed128Math.SHIFT_AMOUNT_I);
    }

    /// <summary>
    /// Divides an integer by a Fixed128
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 operator /(long y, Fixed128 x)
    {
        return new Fixed128((Int128)y << Fixed128Math.SHIFT_AMOUNT_I) / x;
    }

    /// <summary>
    /// Computes the remainder of division of one Fixed128 number by another.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 operator %(Fixed128 x, Fixed128 y)
    {
        if (x.rawValue == Fixed128Math.MIN_VALUE_L && y.rawValue == -1)
            return Zero;
        return new Fixed128(x.rawValue % y.rawValue);
    }

    /// <summary>
    /// Unary negation operator.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 operator -(Fixed128 x)
    {
        return x.rawValue == Fixed128Math.MIN_VALUE_L 
            ? new Fixed128(Fixed128Math.MAX_VALUE_L) 
            : new Fixed128(-x.rawValue);
    }

    /// <summary>
    /// Pre-increment operator (++x).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 operator ++(Fixed128 a)
    {
        return a + One;
    }

    /// <summary>
    /// Pre-decrement operator (--x).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 operator --(Fixed128 a)
    {
        return a - One;
    }

    /// <summary>
    /// Bitwise left shift operator.
    /// </summary>
    /// <param name="a">Operand to shift.</param>
    /// <param name="shift">Number of bits to shift.</param>
    /// <returns>The shifted value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 operator <<(Fixed128 a, int shift)
    {
        return new Fixed128(a.rawValue << shift);
    }

    /// <summary>
    /// Bitwise right shift operator.
    /// </summary>
    /// <param name="a">Operand to shift.</param>
    /// <param name="shift">Number of bits to shift.</param>
    /// <returns>The shifted value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 operator >>(Fixed128 a, int shift)
    {
        return new Fixed128(a.rawValue >> shift);
    }

    #endregion

    #region Comparison Operators

    /// <summary>
    /// Determines whether one Fixed128 is greater than another.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(Fixed128 x, Fixed128 y)
    {
        return x.rawValue > y.rawValue;
    }

    /// <summary>
    /// Determines whether one Fixed128 is less than another.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(Fixed128 x, Fixed128 y)
    {
        return x.rawValue < y.rawValue;
    }

    /// <summary>
    /// Determines whether one Fixed128 is greater than or equal to another.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(Fixed128 x, Fixed128 y)
    {
        return x.rawValue >= y.rawValue;
    }

    /// <summary>
    /// Determines whether one Fixed128 is less than or equal to another.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(Fixed128 x, Fixed128 y)
    {
        return x.rawValue <= y.rawValue;
    }

    /// <summary>
    /// Determines whether two Fixed128 instances are equal.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Fixed128 left, Fixed128 right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two Fixed128 instances are not equal.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Fixed128 left, Fixed128 right)
    {
        return !left.Equals(right);
    }

    #endregion

    #region Conversion

    /// <summary>
    /// Returns the string representation of this Fixed128 instance.
    /// </summary>
    /// <remarks>
    /// Up to 10 decimal places.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return ((decimal)this).ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Converts the numeric value of the current Fixed128 object to its equivalent string representation.
    /// </summary>
    /// <param name="format">A format specification that governs how the current Fixed128 object is converted.</param>
    /// <returns>The string representation of the value of the current Fixed128 object.</returns>  
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string format)
    {
        return ((decimal)this).ToString(format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Parses a string to create a Fixed128 instance.
    /// </summary>
    /// <param name="s">The string representation of the </param>
    /// <returns>The parsed Fixed128 value.</returns>
    public static Fixed128 Parse(string s)
    {
        if (string.IsNullOrEmpty(s)) throw new ArgumentNullException(nameof(s));

        // Check if the value is negative
        bool isNegative = false;
        if (s[0] == '-')
        {
            isNegative = true;
            s = s.Substring(1);
        }

        if (!Int128.TryParse(s, out Int128 rawValue))
            throw new FormatException($"Invalid format: {s}");

        // If the value was negative, negate the result
        if (isNegative)
            rawValue = -rawValue;

        return FromRaw(rawValue);
    }

    /// <summary>
    /// Tries to parse a string to create a Fixed128 instance.
    /// </summary>
    /// <param name="s">The string representation of the </param>
    /// <param name="result">The parsed Fixed128 value.</param>
    /// <returns>True if parsing succeeded; otherwise, false.</returns>
    public static bool TryParse(string s, out Fixed128 result)
    {
        result = Zero;
        if (string.IsNullOrEmpty(s)) return false;

        // Check if the value is negative
        bool isNegative = false;
        if (s[0] == '-')
        {
            isNegative = true;
            s = s.Substring(1);
        }

        if (!Int128.TryParse(s, out Int128 rawValue)) return false;

        // If the value was negative, negate the result
        if (isNegative)
            rawValue = -rawValue;

        result = FromRaw(rawValue);
        return true;
    }

    /// <summary>
    /// Creates a Fixed128 from a raw Int128 value.
    /// </summary>
    /// <param name="rawValue">The raw Int128 value.</param>
    /// <returns>A Fixed128 representing the raw value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Fixed128 FromRaw(Int128 rawValue)
    {
        return new Fixed128(rawValue);
    }

    /// <summary>
    /// Converts a Fixed128s RawValue (Int64) into a double
    /// </summary>
    /// <param name="f1"></param>
    /// <returns></returns>
    public static double ToDouble(Int128 f1)
    {
        return (double)f1 * Fixed128Math.SCALE_FACTOR_D;
    }

    /// <summary>
    /// Converts a Fixed128s RawValue (Int64) into a float
    /// </summary>
    /// <param name="f1"></param>
    /// <returns></returns>
    public static float ToFloat(Int128 f1)
    {
        return (float)f1 * Fixed128Math.SCALE_FACTOR_F;
    }

    /// <summary>
    /// Converts a Fixed128s RawValue (Int64) into a decimal
    /// </summary>
    /// <param name="f1"></param>
    /// <returns></returns>
    public static decimal ToDecimal(Int128 f1)
    {
        return (decimal)f1 * Fixed128Math.SCALE_FACTOR_M;
    }

    #endregion

    #region Equality, HashCode, Comparable Overrides

    /// <summary>
    /// Determines whether this instance equals another object.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        return obj is Fixed128 other && Equals(other);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Fixed128 other)
    {
        return rawValue == other.rawValue;
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Fixed128 x, Fixed128 y)
    {
        return x.Equals(y);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return rawValue.GetHashCode();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetHashCode(Fixed128 obj)
    {
        return obj.GetHashCode();
    }

    /// <summary>
    /// Compares this instance to another 
    /// </summary>
    /// <param name="other">The Fixed128 to compare with.</param>
    /// <returns>-1 if less than, 0 if equal, 1 if greater than other.</returns>
    public int CompareTo(Fixed128 other)
    {
        return rawValue.CompareTo(other.rawValue);
    }

    #endregion
}