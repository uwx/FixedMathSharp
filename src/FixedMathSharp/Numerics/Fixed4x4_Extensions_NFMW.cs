using System.Text.Json.Serialization;
using MessagePack;

namespace FixedMathSharp;

internal static class Fixed4x4Ex
{
    extension(Fixed4x4 m)
    {
        public Fixed64 M11 => m.m00;
        public Fixed64 M12 => m.m01;
        public Fixed64 M13 => m.m02;
        public Fixed64 M14 => m.m03;

        public Fixed64 M21 => m.m10;
        public Fixed64 M22 => m.m11;
        public Fixed64 M23 => m.m12;
        public Fixed64 M24 => m.m13;

        public Fixed64 M31 => m.m20;
        public Fixed64 M32 => m.m21;
        public Fixed64 M33 => m.m22;
        public Fixed64 M34 => m.m23;

        public Fixed64 M41 => m.m30;
        public Fixed64 M42 => m.m31;
        public Fixed64 M43 => m.m32;
        public Fixed64 M44 => m.m33;
    }
}