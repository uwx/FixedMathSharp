using System.Runtime.CompilerServices;

namespace FixedMathSharp
{
    internal static class IBoundExtensions
    {
        /// <inheritdoc cref="IBound.ProjectPoint(Vector3d)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d ProjectPointWithinBounds(this IBound bounds, Vector3d point)
        {
            return new Vector3d(
                FixedMath.Clamp(point.X, bounds.Min.X, bounds.Max.X),
                FixedMath.Clamp(point.Y, bounds.Min.Y, bounds.Max.Y),
                FixedMath.Clamp(point.Z, bounds.Min.Z, bounds.Max.Z)
            );
        }
    }
}
