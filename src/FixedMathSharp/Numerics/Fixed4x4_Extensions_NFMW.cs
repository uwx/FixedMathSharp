using System.Text.Json.Serialization;
using MessagePack;

namespace FixedMathSharp;

internal static class Fixed4x4Ex
{
    extension(Fixed4x4 m)
    {
        public Fixed64 M11 { get => m.m00; set => m.m00 = value; }
        public Fixed64 M12 { get => m.m01; set => m.m01 = value; }
        public Fixed64 M13 { get => m.m02; set => m.m02 = value; }
        public Fixed64 M14 { get => m.m03; set => m.m03 = value; }

        public Fixed64 M21 { get => m.m10; set => m.m10 = value; }
        public Fixed64 M22 { get => m.m11; set => m.m11 = value; }
        public Fixed64 M23 { get => m.m12; set => m.m12 = value; }
        public Fixed64 M24 { get => m.m13; set => m.m13 = value; }

        public Fixed64 M31 { get => m.m20; set => m.m20 = value; }
        public Fixed64 M32 { get => m.m21; set => m.m21 = value; }
        public Fixed64 M33 { get => m.m22; set => m.m22 = value; }
        public Fixed64 M34 { get => m.m23; set => m.m23 = value; }

        public Fixed64 M41 { get => m.m30; set => m.m30 = value; }
        public Fixed64 M42 { get => m.m31; set => m.m31 = value; }
        public Fixed64 M43 { get => m.m32; set => m.m32 = value; }
        public Fixed64 M44 { get => m.m33; set => m.m33 = value; }
    }
}