using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace FixedMathSharp;

public partial struct Fixed64 : ISpanParsable<Fixed64>
{
    public static Fixed64 Parse(string s, IFormatProvider? provider)
    {
        return (Fixed64)float.Parse(s, provider);
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Fixed64 result)
    {
        var success = float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out var temp);
        result = (Fixed64)temp;
        return success;
    }

    public static Fixed64 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        return (Fixed64)float.Parse(s, provider);
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Fixed64 result)
    {
        var success = float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out var temp);
        result = (Fixed64)temp;
        return success;
    }
}