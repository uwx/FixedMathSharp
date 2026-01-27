using MessagePack;

namespace FixedMathSharp;

public partial struct Fixed4x4
{
    [IgnoreMember] public Fixed64 M11 => m00;
    [IgnoreMember] public Fixed64 M12 => m01;
    [IgnoreMember] public Fixed64 M13 => m02;
    [IgnoreMember] public Fixed64 M14 => m03;

    [IgnoreMember] public Fixed64 M21 => m10;
    [IgnoreMember] public Fixed64 M22 => m11;
    [IgnoreMember] public Fixed64 M23 => m12;
    [IgnoreMember] public Fixed64 M24 => m13;

    [IgnoreMember] public Fixed64 M31 => m20;
    [IgnoreMember] public Fixed64 M32 => m21;
    [IgnoreMember] public Fixed64 M33 => m22;
    [IgnoreMember] public Fixed64 M34 => m23;

    [IgnoreMember] public Fixed64 M41 => m30;
    [IgnoreMember] public Fixed64 M42 => m31;
    [IgnoreMember] public Fixed64 M43 => m32;
    [IgnoreMember] public Fixed64 M44 => m33;
}