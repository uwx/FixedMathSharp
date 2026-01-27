using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace FixedMathSharp;

public partial struct Vector3d
{
	/// <summary>
	/// Returns the length of this <see cref="Vector3d"/>.
	/// </summary>
	/// <returns>The length of this <see cref="Vector3d"/>.</returns>
	public Fixed64 Length()
	{
		return (Fixed64) Fixed64.Sqrt((X * X) + (Y * Y) + (Z * Z));
	}

	/// <summary>
	/// Returns the squared length of this <see cref="Vector3d"/>.
	/// </summary>
	/// <returns>The squared length of this <see cref="Vector3d"/>.</returns>
	public Fixed64 LengthSquared()
	{
		return (X * X) + (Y * Y) + (Z * Z);
	}

	#region Internal Methods

	[Conditional("DEBUG")]
	internal void CheckForNaNs()
	{
	}

	#endregion

	#region Public Static Methods

	/// <summary>
	/// Performs vector addition on <paramref name="value1"/> and <paramref name="value2"/>.
	/// </summary>
	/// <param name="value1">The first vector to add.</param>
	/// <param name="value2">The second vector to add.</param>
	/// <returns>The result of the vector addition.</returns>
	public static Vector3d Add(Vector3d value1, Vector3d value2)
	{
		value1.X += value2.X;
		value1.Y += value2.Y;
		value1.Z += value2.Z;
		return value1;
	}

	/// <summary>
	/// Performs vector addition on <paramref name="value1"/> and
	/// <paramref name="value2"/>, storing the result of the
	/// addition in <paramref name="result"/>.
	/// </summary>
	/// <param name="value1">The first vector to add.</param>
	/// <param name="value2">The second vector to add.</param>
	/// <param name="result">The result of the vector addition.</param>
	public static void Add(ref Vector3d value1, ref Vector3d value2, out Vector3d result)
	{
		result.X = value1.X + value2.X;
		result.Y = value1.Y + value2.Y;
		result.Z = value1.Z + value2.Z;
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 3d-triangle.
	/// </summary>
	/// <param name="value1">The first vector of 3d-triangle.</param>
	/// <param name="value2">The second vector of 3d-triangle.</param>
	/// <param name="value3">The third vector of 3d-triangle.</param>
	/// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 3d-triangle.</param>
	/// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 3d-triangle.</param>
	/// <returns>The cartesian translation of barycentric coordinates.</returns>
	public static Vector3d Barycentric(
		Vector3d value1,
		Vector3d value2,
		Vector3d value3,
		Fixed64 amount1,
		Fixed64 amount2
	) {
		return new Vector3d(
			Fixed64.Barycentric(value1.X, value2.X, value3.X, amount1, amount2),
			Fixed64.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2),
			Fixed64.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2)
		);
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains the cartesian coordinates of a vector specified in barycentric coordinates and relative to 3d-triangle.
	/// </summary>
	/// <param name="value1">The first vector of 3d-triangle.</param>
	/// <param name="value2">The second vector of 3d-triangle.</param>
	/// <param name="value3">The third vector of 3d-triangle.</param>
	/// <param name="amount1">Barycentric scalar <c>b2</c> which represents a weighting factor towards second vector of 3d-triangle.</param>
	/// <param name="amount2">Barycentric scalar <c>b3</c> which represents a weighting factor towards third vector of 3d-triangle.</param>
	/// <param name="result">The cartesian translation of barycentric coordinates as an output parameter.</param>
	public static void Barycentric(
		ref Vector3d value1,
		ref Vector3d value2,
		ref Vector3d value3,
		Fixed64 amount1,
		Fixed64 amount2,
		out Vector3d result
	) {
		result.X = Fixed64.Barycentric(value1.X, value2.X, value3.X, amount1, amount2);
		result.Y = Fixed64.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2);
		result.Z = Fixed64.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2);
	}
	
	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains CatmullRom interpolation of the specified vectors.
	/// </summary>
	/// <param name="value1">The first vector in interpolation.</param>
	/// <param name="value2">The second vector in interpolation.</param>
	/// <param name="value3">The third vector in interpolation.</param>
	/// <param name="value4">The fourth vector in interpolation.</param>
	/// <param name="amount">Weighting factor.</param>
	/// <returns>The result of CatmullRom interpolation.</returns>
	public static Vector3d CatmullRom(
		Vector3d value1,
		Vector3d value2,
		Vector3d value3,
		Vector3d value4,
		Fixed64 amount
	) {
		return new Vector3d(
			Fixed64.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
			Fixed64.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount),
			Fixed64.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount)
		);
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains CatmullRom interpolation of the specified vectors.
	/// </summary>
	/// <param name="value1">The first vector in interpolation.</param>
	/// <param name="value2">The second vector in interpolation.</param>
	/// <param name="value3">The third vector in interpolation.</param>
	/// <param name="value4">The fourth vector in interpolation.</param>
	/// <param name="amount">Weighting factor.</param>
	/// <param name="result">The result of CatmullRom interpolation as an output parameter.</param>
	public static void CatmullRom(
		ref Vector3d value1,
		ref Vector3d value2,
		ref Vector3d value3,
		ref Vector3d value4,
		Fixed64 amount,
		out Vector3d result
	) {
		result.X = Fixed64.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount);
		result.Y = Fixed64.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount);
		result.Z = Fixed64.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount);
	}

	/// <summary>
	/// Clamps the specified value within a range.
	/// </summary>
	/// <param name="value1">The value to clamp.</param>
	/// <param name="min">The min value.</param>
	/// <param name="max">The max value.</param>
	/// <param name="result">The clamped value as an output parameter.</param>
	public static void Clamp(
		ref Vector3d value1,
		ref Vector3d min,
		ref Vector3d max,
		out Vector3d result
	) {
		result.X = Fixed64.Clamp(value1.X, min.X, max.X);
		result.Y = Fixed64.Clamp(value1.Y, min.Y, max.Y);
		result.Z = Fixed64.Clamp(value1.Z, min.Z, max.Z);
	}

	/// <summary>
	/// Computes the cross product of two vectors.
	/// </summary>
	/// <param name="vector1">The first vector.</param>
	/// <param name="vector2">The second vector.</param>
	/// <param name="result">The cross product of two vectors as an output parameter.</param>
	public static void Cross(ref Vector3d vector1, ref Vector3d vector2, out Vector3d result)
	{
		Fixed64 x = vector1.Y * vector2.Z - vector2.Y * vector1.Z;
		Fixed64 y = -(vector1.X * vector2.Z - vector2.X * vector1.Z);
		Fixed64 z = vector1.X * vector2.Y - vector2.X * vector1.Y;
		result.X = x;
		result.Y = y;
		result.Z = z;
	}

	/// <summary>
	/// Returns the distance between two vectors.
	/// </summary>
	/// <param name="value1">The first vector.</param>
	/// <param name="value2">The second vector.</param>
	/// <param name="result">The distance between two vectors as an output parameter.</param>
	public static void Distance(ref Vector3d value1, ref Vector3d value2, out Fixed64 result)
	{
		DistanceSquared(ref value1, ref value2, out result);
		result = (Fixed64) Fixed64.Sqrt(result);
	}

	/// <summary>
	/// Returns the squared distance between two vectors.
	/// </summary>
	/// <param name="value1">The first vector.</param>
	/// <param name="value2">The second vector.</param>
	/// <returns>The squared distance between two vectors.</returns>
	public static Fixed64 DistanceSquared(Vector3d value1, Vector3d value2)
	{
		return (
			(value1.X - value2.X) * (value1.X - value2.X) +
			(value1.Y - value2.Y) * (value1.Y - value2.Y) +
			(value1.Z - value2.Z) * (value1.Z - value2.Z)
		);
	}

	/// <summary>
	/// Returns the squared distance between two vectors.
	/// </summary>
	/// <param name="value1">The first vector.</param>
	/// <param name="value2">The second vector.</param>
	/// <param name="result">The squared distance between two vectors as an output parameter.</param>
	public static void DistanceSquared(
		ref Vector3d value1,
		ref Vector3d value2,
		out Fixed64 result
	) {
		result = (
			(value1.X - value2.X) * (value1.X - value2.X) +
			(value1.Y - value2.Y) * (value1.Y - value2.Y) +
			(value1.Z - value2.Z) * (value1.Z - value2.Z)
		);
	}

	/// <summary>
	/// Divides the components of a <see cref="Vector3d"/> by the components of another <see cref="Vector3d"/>.
	/// </summary>
	/// <param name="value1">Source <see cref="Vector3d"/>.</param>
	/// <param name="value2">Divisor <see cref="Vector3d"/>.</param>
	/// <returns>The result of dividing the vectors.</returns>
	public static Vector3d Divide(Vector3d value1, Vector3d value2)
	{
		value1.X /= value2.X;
		value1.Y /= value2.Y;
		value1.Z /= value2.Z;
		return value1;
	}

	/// <summary>
	/// Divides the components of a <see cref="Vector3d"/> by the components of another <see cref="Vector3d"/>.
	/// </summary>
	/// <param name="value1">Source <see cref="Vector3d"/>.</param>
	/// <param name="value2">Divisor <see cref="Vector3d"/>.</param>
	/// <param name="result">The result of dividing the vectors as an output parameter.</param>
	public static void Divide(ref Vector3d value1, ref Vector3d value2, out Vector3d result)
	{
		result.X = value1.X / value2.X;
		result.Y = value1.Y / value2.Y;
		result.Z = value1.Z / value2.Z;
	}

	/// <summary>
	/// Divides the components of a <see cref="Vector3d"/> by a scalar.
	/// </summary>
	/// <param name="value1">Source <see cref="Vector3d"/>.</param>
	/// <param name="value2">Divisor scalar.</param>
	/// <returns>The result of dividing a vector by a scalar.</returns>
	public static Vector3d Divide(Vector3d value1, Fixed64 value2)
	{
		value1.X /= value2;
		value1.Y /= value2;
		value1.Z /= value2;
		return value1;
	}

	/// <summary>
	/// Divides the components of a <see cref="Vector3d"/> by a scalar.
	/// </summary>
	/// <param name="value1">Source <see cref="Vector3d"/>.</param>
	/// <param name="value2">Divisor scalar.</param>
	/// <param name="result">The result of dividing a vector by a scalar as an output parameter.</param>
	public static void Divide(ref Vector3d value1, Fixed64 value2, out Vector3d result)
	{
		result.X = value1.X / value2;
		result.Y = value1.Y / value2;
		result.Z = value1.Z / value2;
	}

	/// <summary>
	/// Returns a dot product of two vectors.
	/// </summary>
	/// <param name="vector1">The first vector.</param>
	/// <param name="vector2">The second vector.</param>
	/// <param name="result">The dot product of two vectors as an output parameter.</param>
	public static void Dot(ref Vector3d vector1, ref Vector3d vector2, out Fixed64 result)
	{
		result = (
			(vector1.X * vector2.X) +
			(vector1.Y * vector2.Y) +
			(vector1.Z * vector2.Z)
		);
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains hermite spline interpolation.
	/// </summary>
	/// <param name="value1">The first position vector.</param>
	/// <param name="tangent1">The first tangent vector.</param>
	/// <param name="value2">The second position vector.</param>
	/// <param name="tangent2">The second tangent vector.</param>
	/// <param name="amount">Weighting factor.</param>
	/// <returns>The hermite spline interpolation vector.</returns>
	public static Vector3d Hermite(
		Vector3d value1,
		Vector3d tangent1,
		Vector3d value2,
		Vector3d tangent2,
		Fixed64 amount
	) {
		Vector3d result = new Vector3d();
		Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
		return result;
	}

	/// <summary>
	/// Performs a Hermite spline interpolation.
	/// </summary>
	/// <param name="value1">Source position.</param>
	/// <param name="tangent1">Source tangent.</param>
	/// <param name="value2">Source position.</param>
	/// <param name="tangent2">Source tangent.</param>
	/// <param name="amount">Weighting factor.</param>
	/// <returns>The result of the Hermite spline interpolation.</returns>
	public static Fixed64 Hermite(
		Fixed64 value1,
		Fixed64 tangent1,
		Fixed64 value2,
		Fixed64 tangent2,
		Fixed64 amount
	) {
		/* All transformed to double not to lose precision
		 * Otherwise, for high numbers of param:amount the result is NaN instead
		 * of Infinity.
		 */
		Fixed64 v1 = value1, v2 = value2, t1 = tangent1, t2 = tangent2, s = amount;
		Fixed64 result;
		Fixed64 sCubed = s * s * s;
		Fixed64 sSquared = s * s;

		if (Fixed64.WithinEpsilon(amount, (Fixed64)0f))
		{
			result = value1;
		}
		else if (Fixed64.WithinEpsilon(amount, (Fixed64)1f))
		{
			result = value2;
		}
		else
		{
			result = (
				((2 * v1 - 2 * v2 + t2 + t1) * sCubed) +
				((3 * v2 - 3 * v1 - 2 * t1 - t2) * sSquared) +
				(t1 * s) +
				v1
			);
		}

		return (Fixed64) result;
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains hermite spline interpolation.
	/// </summary>
	/// <param name="value1">The first position vector.</param>
	/// <param name="tangent1">The first tangent vector.</param>
	/// <param name="value2">The second position vector.</param>
	/// <param name="tangent2">The second tangent vector.</param>
	/// <param name="amount">Weighting factor.</param>
	/// <param name="result">The hermite spline interpolation vector as an output parameter.</param>
	public static void Hermite(
		ref Vector3d value1,
		ref Vector3d tangent1,
		ref Vector3d value2,
		ref Vector3d tangent2,
		Fixed64 amount,
		out Vector3d result
	) {
		result.X = Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
		result.Y = Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
		result.Z = Hermite(value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount);
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains linear interpolation of the specified vectors.
	/// </summary>
	/// <param name="value1">The first vector.</param>
	/// <param name="value2">The second vector.</param>
	/// <param name="amount">Weighting value(between 0.0 and 1.0).</param>
	/// <param name="result">The result of linear interpolation of the specified vectors as an output parameter.</param>
	public static void Lerp(
		ref Vector3d value1,
		ref Vector3d value2,
		Fixed64 amount,
		out Vector3d result
	) {
		result.X = Fixed64.Lerp(value1.X, value2.X, amount);
		result.Y = Fixed64.Lerp(value1.Y, value2.Y, amount);
		result.Z = Fixed64.Lerp(value1.Z, value2.Z, amount);
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains a maximal values from the two vectors.
	/// </summary>
	/// <param name="value1">The first vector.</param>
	/// <param name="value2">The second vector.</param>
	/// <param name="result">The <see cref="Vector3d"/> with maximal values from the two vectors as an output parameter.</param>
	public static void Max(ref Vector3d value1, ref Vector3d value2, out Vector3d result)
	{
		result.X = Fixed64.Max(value1.X, value2.X);
		result.Y = Fixed64.Max(value1.Y, value2.Y);
		result.Z = Fixed64.Max(value1.Z, value2.Z);
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains a minimal values from the two vectors.
	/// </summary>
	/// <param name="value1">The first vector.</param>
	/// <param name="value2">The second vector.</param>
	/// <param name="result">The <see cref="Vector3d"/> with minimal values from the two vectors as an output parameter.</param>
	public static void Min(ref Vector3d value1, ref Vector3d value2, out Vector3d result)
	{
		result.X = Fixed64.Min(value1.X, value2.X);
		result.Y = Fixed64.Min(value1.Y, value2.Y);
		result.Z = Fixed64.Min(value1.Z, value2.Z);
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains a multiplication of two vectors.
	/// </summary>
	/// <param name="value1">Source <see cref="Vector3d"/>.</param>
	/// <param name="value2">Source <see cref="Vector3d"/>.</param>
	/// <returns>The result of the vector multiplication.</returns>
	public static Vector3d Multiply(Vector3d value1, Vector3d value2)
	{
		value1.X *= value2.X;
		value1.Y *= value2.Y;
		value1.Z *= value2.Z;
		return value1;
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains a multiplication of <see cref="Vector3d"/> and a scalar.
	/// </summary>
	/// <param name="value1">Source <see cref="Vector3d"/>.</param>
	/// <param name="scaleFactor">Scalar value.</param>
	/// <returns>The result of the vector multiplication with a scalar.</returns>
	public static Vector3d Multiply(Vector3d value1, Fixed64 scaleFactor)
	{
		value1.X *= scaleFactor;
		value1.Y *= scaleFactor;
		value1.Z *= scaleFactor;
		return value1;
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains a multiplication of <see cref="Vector3d"/> and a scalar.
	/// </summary>
	/// <param name="value1">Source <see cref="Vector3d"/>.</param>
	/// <param name="scaleFactor">Scalar value.</param>
	/// <param name="result">The result of the multiplication with a scalar as an output parameter.</param>
	public static void Multiply(ref Vector3d value1, Fixed64 scaleFactor, out Vector3d result)
	{
		result.X = value1.X * scaleFactor;
		result.Y = value1.Y * scaleFactor;
		result.Z = value1.Z * scaleFactor;
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains a multiplication of two vectors.
	/// </summary>
	/// <param name="value1">Source <see cref="Vector3d"/>.</param>
	/// <param name="value2">Source <see cref="Vector3d"/>.</param>
	/// <param name="result">The result of the vector multiplication as an output parameter.</param>
	public static void Multiply(ref Vector3d value1, ref Vector3d value2, out Vector3d result)
	{
		result.X = value1.X * value2.X;
		result.Y = value1.Y * value2.Y;
		result.Z = value1.Z * value2.Z;
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains the specified vector inversion.
	/// </summary>
	/// <param name="value">Source <see cref="Vector3d"/>.</param>
	/// <returns>The result of the vector inversion.</returns>
	public static Vector3d Negate(Vector3d value)
	{
		value = new Vector3d(-value.X, -value.Y, -value.Z);
		return value;
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains the specified vector inversion.
	/// </summary>
	/// <param name="value">Source <see cref="Vector3d"/>.</param>
	/// <param name="result">The result of the vector inversion as an output parameter.</param>
	public static void Negate(ref Vector3d value, out Vector3d result)
	{
		result.X = -value.X;
		result.Y = -value.Y;
		result.Z = -value.Z;
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains a normalized values from another vector.
	/// </summary>
	/// <param name="value">Source <see cref="Vector3d"/>.</param>
	/// <returns>Unit vector.</returns>
	public static Vector3d Normalize(Vector3d value)
	{
		Fixed64 factor = (Fixed64)1.0f / (Fixed64) Fixed64.Sqrt(
			(value.X * value.X) +
			(value.Y * value.Y) +
			(value.Z * value.Z)
		);
		return new Vector3d(
			value.X * factor,
			value.Y * factor,
			value.Z * factor
		);
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains a normalized values from another vector.
	/// </summary>
	/// <param name="value">Source <see cref="Vector3d"/>.</param>
	/// <param name="result">Unit vector as an output parameter.</param>
	public static void Normalize(ref Vector3d value, out Vector3d result)
	{
		Fixed64 factor = (Fixed64)1.0f / (Fixed64) Fixed64.Sqrt(
			(value.X * value.X) +
			(value.Y * value.Y) +
			(value.Z * value.Z)
		);
		result.X = value.X * factor;
		result.Y = value.Y * factor;
		result.Z = value.Z * factor;
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains reflect vector of the given vector and normal.
	/// </summary>
	/// <param name="vector">Source <see cref="Vector3d"/>.</param>
	/// <param name="normal">Reflection normal.</param>
	/// <returns>Reflected vector.</returns>
	public static Vector3d Reflect(Vector3d vector, Vector3d normal)
	{
		/* I is the original array.
		 * N is the normal of the incident plane.
		 * R = I - (2 * N * ( DotProduct[ I,N] ))
		 */
		Vector3d reflectedVector;
		// Inline the dotProduct here instead of calling method
		Fixed64 dotProduct = ((vector.X * normal.X) + (vector.Y * normal.Y)) +
					(vector.Z * normal.Z);
		reflectedVector.X = vector.X - ((Fixed64)2.0f * normal.X) * dotProduct;
		reflectedVector.Y = vector.Y - ((Fixed64)2.0f * normal.Y) * dotProduct;
		reflectedVector.Z = vector.Z - ((Fixed64)2.0f * normal.Z) * dotProduct;

		return reflectedVector;
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains reflect vector of the given vector and normal.
	/// </summary>
	/// <param name="vector">Source <see cref="Vector3d"/>.</param>
	/// <param name="normal">Reflection normal.</param>
	/// <param name="result">Reflected vector as an output parameter.</param>
	public static void Reflect(ref Vector3d vector, ref Vector3d normal, out Vector3d result)
	{
		/* I is the original array.
		 * N is the normal of the incident plane.
		 * R = I - (2 * N * ( DotProduct[ I,N] ))
		 */

		// Inline the dotProduct here instead of calling method.
		Fixed64 dotProduct = ((vector.X * normal.X) + (vector.Y * normal.Y)) +
					(vector.Z * normal.Z);
		result.X = vector.X - ((Fixed64)2.0f * normal.X) * dotProduct;
		result.Y = vector.Y - ((Fixed64)2.0f * normal.Y) * dotProduct;
		result.Z = vector.Z - ((Fixed64)2.0f * normal.Z) * dotProduct;

	}

	/// <summary>
	/// Interpolates between two values using a cubic equation.
	/// </summary>
	/// <param name="value1">Source value.</param>
	/// <param name="value2">Source value.</param>
	/// <param name="amount">Weighting value.</param>
	/// <returns>Interpolated value.</returns>
	private static Fixed64 SmoothStep(Fixed64 value1, Fixed64 value2, Fixed64 amount)
	{
		/* It is expected that 0 < amount < 1.
		 * If amount < 0, return value1.
		 * If amount > 1, return value2.
		 */
		Fixed64 result = Fixed64.Clamp(amount, (Fixed64)0f, (Fixed64)1f);
		result = Hermite(value1, (Fixed64)0f, value2, (Fixed64)0f, result);

		return result;
	}
	
	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains cubic interpolation of the specified vectors.
	/// </summary>
	/// <param name="value1">Source <see cref="Vector3d"/>.</param>
	/// <param name="value2">Source <see cref="Vector3d"/>.</param>
	/// <param name="amount">Weighting value.</param>
	/// <returns>Cubic interpolation of the specified vectors.</returns>
	public static Vector3d SmoothStep(Vector3d value1, Vector3d value2, Fixed64 amount)
	{
		return new Vector3d(
			SmoothStep(value1.X, value2.X, amount),
			SmoothStep(value1.Y, value2.Y, amount),
			SmoothStep(value1.Z, value2.Z, amount)
		);
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains cubic interpolation of the specified vectors.
	/// </summary>
	/// <param name="value1">Source <see cref="Vector3d"/>.</param>
	/// <param name="value2">Source <see cref="Vector3d"/>.</param>
	/// <param name="amount">Weighting value.</param>
	/// <param name="result">Cubic interpolation of the specified vectors as an output parameter.</param>
	public static void SmoothStep(
		ref Vector3d value1,
		ref Vector3d value2,
		Fixed64 amount,
		out Vector3d result
	) {
		result.X = SmoothStep(value1.X, value2.X, amount);
		result.Y = SmoothStep(value1.Y, value2.Y, amount);
		result.Z = SmoothStep(value1.Z, value2.Z, amount);
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains subtraction of on <see cref="Vector3d"/> from a another.
	/// </summary>
	/// <param name="value1">Source <see cref="Vector3d"/>.</param>
	/// <param name="value2">Source <see cref="Vector3d"/>.</param>
	/// <returns>The result of the vector subtraction.</returns>
	public static Vector3d Subtract(Vector3d value1, Vector3d value2)
	{
		value1.X -= value2.X;
		value1.Y -= value2.Y;
		value1.Z -= value2.Z;
		return value1;
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains subtraction of on <see cref="Vector3d"/> from a another.
	/// </summary>
	/// <param name="value1">Source <see cref="Vector3d"/>.</param>
	/// <param name="value2">Source <see cref="Vector3d"/>.</param>
	/// <param name="result">The result of the vector subtraction as an output parameter.</param>
	public static void Subtract(ref Vector3d value1, ref Vector3d value2, out Vector3d result)
	{
		result.X = value1.X - value2.X;
		result.Y = value1.Y - value2.Y;
		result.Z = value1.Z - value2.Z;
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains a transformation of 3d-vector by the specified <see cref="Matrix"/>.
	/// </summary>
	/// <param name="position">Source <see cref="Vector3d"/>.</param>
	/// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
	/// <returns>Transformed <see cref="Vector3d"/>.</returns>
	public static Vector3d Transform(Vector3d position, Fixed4x4 matrix)
	{
		Transform(ref position, ref matrix, out position);
		return position;
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains a transformation of 3d-vector by the specified <see cref="Matrix"/>.
	/// </summary>
	/// <param name="position">Source <see cref="Vector3d"/>.</param>
	/// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
	/// <param name="result">Transformed <see cref="Vector3d"/> as an output parameter.</param>
	public static void Transform(
		ref Vector3d position,
		ref Fixed4x4 matrix,
		out Vector3d result
	) {
		Fixed64 x = (
			(position.X * (Fixed64)matrix.M11) +
			(position.Y * (Fixed64)matrix.M21) +
			(position.Z * (Fixed64)matrix.M31) +
			(Fixed64)matrix.M41
		);
		Fixed64 y = (
			(position.X * (Fixed64)matrix.M12) +
			(position.Y * (Fixed64)matrix.M22) +
			(position.Z * (Fixed64)matrix.M32) +
			(Fixed64)matrix.M42
		);
		Fixed64 z = (
			(position.X * (Fixed64)matrix.M13) +
			(position.Y * (Fixed64)matrix.M23) +
			(position.Z * (Fixed64)matrix.M33) +
			(Fixed64)matrix.M43
		);
		result.X = x;
		result.Y = y;
		result.Z = z;
	}

	/// <summary>
	/// Apply transformation on all vectors within array of <see cref="Vector3d"/> by the specified <see cref="Matrix"/> and places the results in an another array.
	/// </summary>
	/// <param name="sourceArray">Source array.</param>
	/// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
	/// <param name="destinationArray">Destination array.</param>
	public static void Transform(
		Vector3d[] sourceArray,
		ref Fixed4x4 matrix,
		Vector3d[] destinationArray
	) {
		Debug.Assert(
			destinationArray.Length >= sourceArray.Length,
			"The destination array is smaller than the source array."
		);

		/* TODO: Are there options on some platforms to implement
		 * a vectorized version of this?
		 */

		for (int i = 0; i < sourceArray.Length; i += 1)
		{
			Vector3d position = sourceArray[i];
			destinationArray[i] = new Vector3d(
				(position.X * (Fixed64)matrix.M11) + (position.Y * (Fixed64)matrix.M21) +
					(position.Z * (Fixed64)matrix.M31) + (Fixed64)matrix.M41,
				(position.X * (Fixed64)matrix.M12) + (position.Y * (Fixed64)matrix.M22) +
					(position.Z * (Fixed64)matrix.M32) + (Fixed64)matrix.M42,
				(position.X * (Fixed64)matrix.M13) + (position.Y * (Fixed64)matrix.M23) +
					(position.Z * (Fixed64)matrix.M33) + (Fixed64)matrix.M43
			);
		}
	}

	/// <summary>
	/// Apply transformation on vectors within array of <see cref="Vector3d"/> by the specified <see cref="Matrix"/> and places the results in an another array.
	/// </summary>
	/// <param name="sourceArray">Source array.</param>
	/// <param name="sourceIndex">The starting index of transformation in the source array.</param>
	/// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
	/// <param name="destinationArray">Destination array.</param>
	/// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vector3d"/> should be written.</param>
	/// <param name="length">The number of vectors to be transformed.</param>
	public static void Transform(
		Vector3d[] sourceArray,
		int sourceIndex,
		ref Fixed4x4 matrix,
		Vector3d[] destinationArray,
		int destinationIndex,
		int length
	) {
		Debug.Assert(
			sourceArray.Length - sourceIndex >= length,
			"The source array is too small for the given sourceIndex and length."
		);
		Debug.Assert(
			destinationArray.Length - destinationIndex >= length,
			"The destination array is too small for " +
			"the given destinationIndex and length."
		);

		/* TODO: Are there options on some platforms to implement a
		 * vectorized version of this?
		 */

		for (int i = 0; i < length; i += 1)
		{
			Vector3d position = sourceArray[sourceIndex + i];
			destinationArray[destinationIndex + i] = new Vector3d(
				(position.X * (Fixed64)matrix.M11) + (position.Y * (Fixed64)matrix.M21) +
					(position.Z * (Fixed64)matrix.M31) + (Fixed64)matrix.M41,
				(position.X * (Fixed64)matrix.M12) + (position.Y * (Fixed64)matrix.M22) +
					(position.Z * (Fixed64)matrix.M32) + (Fixed64)matrix.M42,
				(position.X * (Fixed64)matrix.M13) + (position.Y * (Fixed64)matrix.M23) +
					(position.Z * (Fixed64)matrix.M33) + (Fixed64)matrix.M43
			);
		}
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains a transformation of 3d-vector by the specified <see cref="Quaternion"/>, representing the rotation.
	/// </summary>
	/// <param name="value">Source <see cref="Vector3d"/>.</param>
	/// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
	/// <returns>Transformed <see cref="Vector3d"/>.</returns>
	public static Vector3d Transform(Vector3d value, Quaternion rotation)
	{
		Vector3d result;
		Transform(ref value, ref rotation, out result);
		return result;
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains a transformation of 3d-vector by the specified <see cref="Quaternion"/>, representing the rotation.
	/// </summary>
	/// <param name="value">Source <see cref="Vector3d"/>.</param>
	/// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
	/// <param name="result">Transformed <see cref="Vector3d"/> as an output parameter.</param>
	public static void Transform(
		ref Vector3d value,
		ref Quaternion rotation,
		out Vector3d result
	) {
		Fixed64 x = (Fixed64)2f * ((Fixed64)rotation.Y * value.Z - (Fixed64)rotation.Z * value.Y);
		Fixed64 y = (Fixed64)2f * ((Fixed64)rotation.Z * value.X - (Fixed64)rotation.X * value.Z);
		Fixed64 z = (Fixed64)2f * ((Fixed64)rotation.X * value.Y - (Fixed64)rotation.Y * value.X);

		result.X = value.X + x * (Fixed64)rotation.W + ((Fixed64)rotation.Y * z - (Fixed64)rotation.Z * y);
		result.Y = value.Y + y * (Fixed64)rotation.W + ((Fixed64)rotation.Z * x - (Fixed64)rotation.X * z);
		result.Z = value.Z + z * (Fixed64)rotation.W + ((Fixed64)rotation.X * y - (Fixed64)rotation.Y * x);
	}

	/// <summary>
	/// Apply transformation on all vectors within array of <see cref="Vector3d"/> by the specified <see cref="Quaternion"/> and places the results in an another array.
	/// </summary>
	/// <param name="sourceArray">Source array.</param>
	/// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
	/// <param name="destinationArray">Destination array.</param>
	public static void Transform(
		Vector3d[] sourceArray,
		ref Quaternion rotation,
		Vector3d[] destinationArray
	) {
		Debug.Assert(
			destinationArray.Length >= sourceArray.Length,
			"The destination array is smaller than the source array."
		);

		/* TODO: Are there options on some platforms to implement
		 * a vectorized version of this?
		 */

		for (int i = 0; i < sourceArray.Length; i += 1)
		{
			Vector3d position = sourceArray[i];

			Fixed64 x = (Fixed64)2f * ((Fixed64)rotation.Y * position.Z - (Fixed64)rotation.Z * position.Y);
			Fixed64 y = (Fixed64)2f * ((Fixed64)rotation.Z * position.X - (Fixed64)rotation.X * position.Z);
			Fixed64 z = (Fixed64)2f * ((Fixed64)rotation.X * position.Y - (Fixed64)rotation.Y * position.X);

			destinationArray[i] = new Vector3d(
				position.X + x * (Fixed64)rotation.W + ((Fixed64)rotation.Y * z - (Fixed64)rotation.Z * y),
				position.Y + y * (Fixed64)rotation.W + ((Fixed64)rotation.Z * x - (Fixed64)rotation.X * z),
				position.Z + z * (Fixed64)rotation.W + ((Fixed64)rotation.X * y - (Fixed64)rotation.Y * x)
			);
		}
	}

	/// <summary>

	/// Apply transformation on vectors within array of <see cref="Vector3d"/> by the specified <see cref="Quaternion"/> and places the results in an another array.
	/// </summary>
	/// <param name="sourceArray">Source array.</param>
	/// <param name="sourceIndex">The starting index of transformation in the source array.</param>
	/// <param name="rotation">The <see cref="Quaternion"/> which contains rotation transformation.</param>
	/// <param name="destinationArray">Destination array.</param>
	/// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vector3d"/> should be written.</param>
	/// <param name="length">The number of vectors to be transformed.</param>
	public static void Transform(
		Vector3d[] sourceArray,
		int sourceIndex,
		ref Quaternion rotation,
		Vector3d[] destinationArray,
		int destinationIndex,
		int length
	) {
		Debug.Assert(
			sourceArray.Length - sourceIndex >= length,
			"The source array is too small for the given sourceIndex and length."
		);
		Debug.Assert(
			destinationArray.Length - destinationIndex >= length,
			"The destination array is too small for the " +
			"given destinationIndex and length."
		);

		/* TODO: Are there options on some platforms to implement
		 * a vectorized version of this?
		 */

		for (int i = 0; i < length; i += 1)
		{
			Vector3d position = sourceArray[sourceIndex + i];

			Fixed64 x = 2 * ((Fixed64)rotation.Y * position.Z - (Fixed64)rotation.Z * position.Y);
			Fixed64 y = 2 * ((Fixed64)rotation.Z * position.X - (Fixed64)rotation.X * position.Z);
			Fixed64 z = 2 * ((Fixed64)rotation.X * position.Y - (Fixed64)rotation.Y * position.X);

			destinationArray[destinationIndex + i] = new Vector3d(
				position.X + x * (Fixed64)rotation.W + ((Fixed64)rotation.Y * z - (Fixed64)rotation.Z * y),
				position.Y + y * (Fixed64)rotation.W + ((Fixed64)rotation.Z * x - (Fixed64)rotation.X * z),
				position.Z + z * (Fixed64)rotation.W + ((Fixed64)rotation.X * y - (Fixed64)rotation.Y * x)
			);
		}
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains a transformation of the specified normal by the specified <see cref="Matrix"/>.
	/// </summary>
	/// <param name="normal">Source <see cref="Vector3d"/> which represents a normal vector.</param>
	/// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
	/// <returns>Transformed normal.</returns>
	public static Vector3d TransformNormal(Vector3d normal, Fixed4x4 matrix)
	{
		TransformNormal(ref normal, ref matrix, out normal);
		return normal;
	}

	/// <summary>
	/// Creates a new <see cref="Vector3d"/> that contains a transformation of the specified normal by the specified <see cref="Matrix"/>.
	/// </summary>
	/// <param name="normal">Source <see cref="Vector3d"/> which represents a normal vector.</param>
	/// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
	/// <param name="result">Transformed normal as an output parameter.</param>
	public static void TransformNormal(
		ref Vector3d normal,
		ref Fixed4x4 matrix,
		out Vector3d result
	) {
		Fixed64 x = (normal.X * (Fixed64)matrix.M11) + (normal.Y * (Fixed64)matrix.M21) + (normal.Z * (Fixed64)matrix.M31);
		Fixed64 y = (normal.X * (Fixed64)matrix.M12) + (normal.Y * (Fixed64)matrix.M22) + (normal.Z * (Fixed64)matrix.M32);
		Fixed64 z = (normal.X * (Fixed64)matrix.M13) + (normal.Y * (Fixed64)matrix.M23) + (normal.Z * (Fixed64)matrix.M33);
		result.X = x;
		result.Y = y;
		result.Z = z;
	}

	/// <summary>
	/// Apply transformation on all normals within array of <see cref="Vector3d"/> by the specified <see cref="Matrix"/> and places the results in an another array.
	/// </summary>
	/// <param name="sourceArray">Source array.</param>
	/// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
	/// <param name="destinationArray">Destination array.</param>
	public static void TransformNormal(
		Vector3d[] sourceArray,
		ref Fixed4x4 matrix,
		Vector3d[] destinationArray
	) {
		Debug.Assert(
			destinationArray.Length >= sourceArray.Length,
			"The destination array is smaller than the source array."
		);

		for (int i = 0; i < sourceArray.Length; i += 1)
		{
			Vector3d normal = sourceArray[i];
			destinationArray[i].X = (normal.X * (Fixed64)matrix.M11) + (normal.Y * (Fixed64)matrix.M21) + (normal.Z * (Fixed64)matrix.M31);
			destinationArray[i].Y = (normal.X * (Fixed64)matrix.M12) + (normal.Y * (Fixed64)matrix.M22) + (normal.Z * (Fixed64)matrix.M32);
			destinationArray[i].Z = (normal.X * (Fixed64)matrix.M13) + (normal.Y * (Fixed64)matrix.M23) + (normal.Z * (Fixed64)matrix.M33);
		}
	}

	/// <summary>
	/// Apply transformation on normals within array of <see cref="Vector3d"/> by the specified <see cref="Matrix"/> and places the results in an another array.
	/// </summary>
	/// <param name="sourceArray">Source array.</param>
	/// <param name="sourceIndex">The starting index of transformation in the source array.</param>
	/// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
	/// <param name="destinationArray">Destination array.</param>
	/// <param name="destinationIndex">The starting index in the destination array, where the first <see cref="Vector3d"/> should be written.</param>
	/// <param name="length">The number of normals to be transformed.</param>
	public static void TransformNormal(
		Vector3d[] sourceArray,
		int sourceIndex,
		ref Fixed4x4 matrix,
		Vector3d[] destinationArray,
		int destinationIndex,
		int length
	) {
		if (sourceArray == null)
		{
			throw new ArgumentNullException("sourceArray");
		}
		if (destinationArray == null)
		{
			throw new ArgumentNullException("destinationArray");
		}
		if ((sourceIndex + length) > sourceArray.Length)
		{
			throw new ArgumentException(
				"the combination of sourceIndex and " +
				"length was greater than sourceArray.Length"
			);
		}
		if ((destinationIndex + length) > destinationArray.Length)
		{
			throw new ArgumentException(
				"destinationArray is too small to " +
				"contain the result"
			);
		}

		for (int i = 0; i < length; i += 1)
		{
			Vector3d normal = sourceArray[i + sourceIndex];
			destinationArray[i + destinationIndex].X = (
				(normal.X * (Fixed64)matrix.M11) +
				(normal.Y * (Fixed64)matrix.M21) +
				(normal.Z * (Fixed64)matrix.M31)
			);
			destinationArray[i + destinationIndex].Y = (
				(normal.X * (Fixed64)matrix.M12) +
				(normal.Y * (Fixed64)matrix.M22) +
				(normal.Z * (Fixed64)matrix.M32)
			);
			destinationArray[i + destinationIndex].Z = (
				(normal.X * (Fixed64)matrix.M13) +
				(normal.Y * (Fixed64)matrix.M23) +
				(normal.Z * (Fixed64)matrix.M33)
			);
		}
	}

	#endregion

	public static Vector3d FromSpan(ReadOnlySpan<Fixed64> span)
		=> new(span[0], span[1], span[2]);

	public static explicit operator Vector3(Vector3d value)
		=> new((float)value.X, (float)value.Y, (float)value.Z);
	public static explicit operator Vector3d(Vector3 value)
		=> new((Fixed64)value.X, (Fixed64)value.Y, (Fixed64)value.Z);

	public static explicit operator Microsoft.Xna.Framework.Vector3(Vector3d value)
		=> new((float)value.X, (float)value.Y, (float)value.Z);
	public static explicit operator Vector3d(Microsoft.Xna.Framework.Vector3 value)
		=> new((Fixed64)value.X, (Fixed64)value.Y, (Fixed64)value.Z);

	public Span<Fixed64> AsSpan()
	{
		return MemoryMarshal.CreateSpan(ref Unsafe.As<Vector3d, Fixed64>(ref this), 3);
	}
}