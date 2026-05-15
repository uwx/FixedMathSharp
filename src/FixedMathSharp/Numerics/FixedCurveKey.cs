using MemoryPack;
using System;
using System.Text.Json.Serialization;
using MessagePack;

namespace FixedMathSharp;

/// <summary>
/// Represents a keyframe in a <see cref="FixedCurve"/>, defining a value at a specific time.
/// </summary>
[Serializable]
[MemoryPackable]
[MessagePackObject]
public partial struct FixedCurveKey : IEquatable<FixedCurveKey>
{
    #region Fields

    /// <summary>The time at which this keyframe occurs.</summary>
    [Key(0)]
    [JsonInclude]
    [MemoryPackOrder(0)]
    public Fixed64 Time;

    /// <summary>The value of the curve at this keyframe.</summary>
    [Key(1)]
    [JsonInclude]
    [MemoryPackOrder(1)]
    public Fixed64 Value;

    /// <summary>The incoming tangent for cubic interpolation.</summary>
    [Key(2)]
    [JsonInclude]
    [MemoryPackOrder(2)]
    public Fixed64 InTangent;

    /// <summary>The outgoing tangent for cubic interpolation.</summary>
    [Key(3)]
    [JsonInclude]
    [MemoryPackOrder(3)]
    public Fixed64 OutTangent;

    #endregion

    #region Constructors
    /// <summary>
    /// Creates a keyframe with a specified time and value.
    /// </summary>
    public static FixedCurveKey CreateFromDouble(double time, double value)
        => new(Fixed64.CreateFromDouble(time), Fixed64.CreateFromDouble(value));

    /// <summary>
    /// Creates a keyframe with optional tangents for cubic interpolation.
    /// </summary>
    public static FixedCurveKey CreateFromDouble(double time, double value, double inTangent, double outTangent)
        => new(Fixed64.CreateFromDouble(time), Fixed64.CreateFromDouble(value), Fixed64.CreateFromDouble(inTangent), Fixed64.CreateFromDouble(outTangent));

    /// <summary>
    /// Creates a keyframe with a specified time and value.
    /// </summary>
    public FixedCurveKey(Fixed64 time, Fixed64 value)
        : this(time, value, Fixed64.Zero, Fixed64.Zero) { }

    /// <summary>
    /// Creates a keyframe with optional tangents for cubic interpolation.
    /// </summary>
    [JsonConstructor]
    public FixedCurveKey(Fixed64 time, Fixed64 value, Fixed64 inTangent, Fixed64 outTangent)
    {
        Time = time;
        Value = value;
        InTangent = inTangent;
        OutTangent = outTangent;
    }

    #endregion

    #region Equality

    /// <inheritdoc/>
    public bool Equals(FixedCurveKey other)
    {
        return Time == other.Time &&
               Value == other.Value &&
               InTangent == other.InTangent &&
               OutTangent == other.OutTangent;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is FixedCurveKey other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Time, Value, InTangent, OutTangent);
    }

    /// <summary>
    /// Determines whether two FixedCurveKey instances are equal.
    /// </summary>
    public static bool operator ==(FixedCurveKey left, FixedCurveKey right) => left.Equals(right);

    /// <summary>
    /// Determines whether two FixedCurveKey instances are not equal.
    /// </summary>
    public static bool operator !=(FixedCurveKey left, FixedCurveKey right) => !(left == right);

    #endregion
}