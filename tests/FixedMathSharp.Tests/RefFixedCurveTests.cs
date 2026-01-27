using MessagePack;

#if NET48_OR_GREATER
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
#endif

#if NET8_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;
#endif

using Xunit;

namespace FixedMathSharp.Tests
{
    public class RefFixedCurveTests
    {
        [Fact]
        public void Evaluate_LinearInterpolation_ShouldInterpolateCorrectly()
        {
            RefFixedCurve curve = new RefFixedCurve(FixedCurveMode.Linear, [
                FixedCurveKey.CreateFromDouble(0, 0),
                FixedCurveKey.CreateFromDouble(10, 100)
            ]);

            Assert.Equal(Fixed64.Zero, curve.Evaluate(Fixed64.Zero));
            Assert.Equal((Fixed64)50, curve.Evaluate((Fixed64)5)); // Midpoint
            Assert.Equal((Fixed64)100, curve.Evaluate((Fixed64)10));
        }

        [Fact]
        public void Evaluate_StepInterpolation_ShouldJumpToNearestKeyframe()
        {
            RefFixedCurve curve = new RefFixedCurve(FixedCurveMode.Step, [
                FixedCurveKey.CreateFromDouble(0, 10),
                FixedCurveKey.CreateFromDouble(5, 50),
                FixedCurveKey.CreateFromDouble(10, 100)
            ]);

            Assert.Equal((Fixed64)10, curve.Evaluate(Fixed64.Zero));
            Assert.Equal((Fixed64)10, curve.Evaluate((Fixed64)4.99)); // Should not interpolate
            Assert.Equal((Fixed64)50, curve.Evaluate((Fixed64)5));
            Assert.Equal((Fixed64)50, curve.Evaluate((Fixed64)9.99));
            Assert.Equal((Fixed64)100, curve.Evaluate((Fixed64)10));
        }

        [Fact]
        public void Evaluate_SmoothInterpolation_ShouldSmoothlyTransition()
        {
            RefFixedCurve curve = new RefFixedCurve(FixedCurveMode.Smooth, [
                FixedCurveKey.CreateFromDouble(0, 0),
                FixedCurveKey.CreateFromDouble(10, 100)
            ]);

            Fixed64 result = curve.Evaluate((Fixed64)5);
            Assert.True(result > (Fixed64)45 && result < (Fixed64)55,
                $"Unexpected smooth interpolation result: {result}");
        }

        [Fact]
        public void Evaluate_CubicInterpolation_ShouldUseTangentsCorrectly()
        {
            RefFixedCurve curve = new RefFixedCurve(FixedCurveMode.Cubic, [
                FixedCurveKey.CreateFromDouble(0, 0, 10, 10),
                FixedCurveKey.CreateFromDouble(10, 100, -10, -10)
            ]);

            Fixed64 result = curve.Evaluate((Fixed64)5);
            Assert.True(result > (Fixed64)45 && result < (Fixed64)55,
                $"Unexpected cubic interpolation result: {result}");
        }

        [Fact]
        public void Evaluate_TimeBeforeFirstKeyframe_ShouldReturnFirstValue()
        {
            RefFixedCurve curve = new RefFixedCurve(FixedCurveMode.Linear, [
                FixedCurveKey.CreateFromDouble(5, 50),
                FixedCurveKey.CreateFromDouble(10, 100)
            ]);

            Assert.Equal((Fixed64)50, curve.Evaluate(Fixed64.Zero));
        }

        [Fact]
        public void Evaluate_TimeAfterLastKeyframe_ShouldReturnLastValue()
        {
            RefFixedCurve curve = new RefFixedCurve(FixedCurveMode.Linear, [
                FixedCurveKey.CreateFromDouble(5, 50),
                FixedCurveKey.CreateFromDouble(10, 100)
            ]);

            Assert.Equal((Fixed64)100, curve.Evaluate((Fixed64)15));
        }

        [Fact]
        public void Evaluate_SingleKeyframe_ShouldAlwaysReturnSameValue()
        {
            RefFixedCurve curve = new RefFixedCurve(FixedCurveMode.Linear,
                [FixedCurveKey.CreateFromDouble(5, 50)]);

            Assert.Equal((Fixed64)50, curve.Evaluate(Fixed64.Zero));
            Assert.Equal((Fixed64)50, curve.Evaluate((Fixed64)10));
        }

        [Fact]
        public void Evaluate_DuplicateKeyframes_ShouldHandleGracefully()
        {
            RefFixedCurve curve = new RefFixedCurve(FixedCurveMode.Linear, [
                FixedCurveKey.CreateFromDouble(5, 50),
                FixedCurveKey.CreateFromDouble(5, 50), // Duplicate
                FixedCurveKey.CreateFromDouble(10, 100)
            ]);

            Assert.Equal((Fixed64)50, curve.Evaluate((Fixed64)5));
            Assert.Equal((Fixed64)100, curve.Evaluate((Fixed64)10));
        }

        [Fact]
        public void Evaluate_NegativeValues_ShouldInterpolateCorrectly()
        {
            RefFixedCurve curve = new RefFixedCurve(FixedCurveMode.Linear, [
                FixedCurveKey.CreateFromDouble(-10, -100),
                FixedCurveKey.CreateFromDouble(0, 0),
                FixedCurveKey.CreateFromDouble(10, 100)
            ]);

            Assert.Equal((Fixed64)(-100), curve.Evaluate(-(Fixed64)10));
            Assert.Equal((Fixed64)(0), curve.Evaluate(Fixed64.Zero));
            Assert.Equal((Fixed64)(100), curve.Evaluate((Fixed64)10));
        }

        [Fact]
        public void Evaluate_ExtremeValues_ShouldHandleCorrectly()
        {
            RefFixedCurve curve = new RefFixedCurve(FixedCurveMode.Linear, [
                new FixedCurveKey(Fixed64.MIN_VALUE, -(Fixed64)10000),
                new FixedCurveKey(Fixed64.MAX_VALUE, (Fixed64)10000)
            ]);

            Assert.Equal((Fixed64)(-10000), curve.Evaluate(Fixed64.MIN_VALUE));
            Assert.Equal((Fixed64)(10000), curve.Evaluate(Fixed64.MAX_VALUE));
        }
    }
}