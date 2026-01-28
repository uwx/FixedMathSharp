using System;
using System.Collections.Generic;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;

namespace FixedMathSharp.Benchmarks
{
    [Config(typeof(QuickConfig))]
    public class Benchmarks
    {
        public class QuickConfig : ManualConfig
        {
            public QuickConfig()
            {
                AddJob(Job.ShortRun
                    .WithToolchain(InProcessEmitToolchain.Instance));
            }
        }
        
        public static IEnumerable<object[]> GetFixed64Pairs()
        {
            yield return [new Fixed64(123456789), new Fixed64(987654321)];
            yield return [new Fixed64(-123456789), new Fixed64(987654321)];
            yield return [new Fixed64(123456789), new Fixed64(-987654321)];
            yield return [new Fixed64(-123456789), new Fixed64(-987654321)];
            yield return [new Fixed64(int.MaxValue / 2), new Fixed64(2)];
            yield return [new Fixed64(int.MinValue / 2), new Fixed64(2)];
            yield return [new Fixed64(2), new Fixed64(4)];
            yield return [new Fixed64(2), Fixed64.CreateFromDouble(0.025)];
        }
        
        [Benchmark]
        [ArgumentsSource(nameof(GetFixed64Pairs))]
        public Fixed64 MulInt128(Fixed64 a, Fixed64 b)
        {
            // Widen to 128 bits to prevent overflow during multiplication
            // 128-bit intrinsic is faster than hand rolled multiplication + shift
            var mul = ((Int128)a.m_rawValue * b.m_rawValue) >> FixedMath.SHIFT_AMOUNT_I;
        
            if (mul < long.MinValue)
                mul = long.MinValue;
            else if (mul > long.MaxValue)
                mul = long.MaxValue;

            return Fixed64.CreateRaw((long)mul);
        }

        [Benchmark]
        [ArgumentsSource(nameof(GetFixed64Pairs))]
        public Fixed64 MulBigMul(Fixed64 a, Fixed64 b)
        {
            // Use Math.BigMul to get 128-bit result as high and low parts
            long high = Math.BigMul(a.m_rawValue, b.m_rawValue, out long low);

            // Check the most significant bit that will be dropped for rounding
            ulong roundBit = 1UL << (FixedMath.SHIFT_AMOUNT_I - 1);

            // Combine high and low parts, shifting right by SHIFT_AMOUNT_I
            long result = (high << (64 - FixedMath.SHIFT_AMOUNT_I)) | (low >>> FixedMath.SHIFT_AMOUNT_I);

            // Apply rounding
            if (((ulong)low & roundBit) != 0) result++;

            // Overflow check: if high bits don't match sign extension, clamp
            long signCheck = high >> (FixedMath.SHIFT_AMOUNT_I - 1);
            if (signCheck != 0 && signCheck != -1)
                result = high < 0 ? long.MinValue : long.MaxValue;

            return Fixed64.CreateRaw(result);
        }

        [Benchmark]
        [ArgumentsSource(nameof(GetFixed64Pairs))]
        public Fixed64 MulFixedMathSharp(Fixed64 x, Fixed64 y)
        {
            long xl = x.m_rawValue;
            long yl = y.m_rawValue;

            // Split both numbers into high and low parts
            ulong xlo = (ulong)(xl & FixedMath.MAX_SHIFTED_AMOUNT_UI);
            long xhi = xl >> FixedMath.SHIFT_AMOUNT_I;
            ulong ylo = (ulong)(yl & FixedMath.MAX_SHIFTED_AMOUNT_UI);
            long yhi = yl >> FixedMath.SHIFT_AMOUNT_I;

            // Perform partial products
            ulong lolo = xlo * ylo; // low bits * low bits
            long lohi = (long)xlo * yhi; // low bits * high bits
            long hilo = xhi * (long)ylo; // high bits * low bits
            long hihi = xhi * yhi; // high bits * high bits

            // Combine results, starting with the low part
            ulong loResult = lolo >> FixedMath.SHIFT_AMOUNT_I;
            long hiResult = hihi << FixedMath.SHIFT_AMOUNT_I;

            // Adjust rounding for the fractional part of the lolo term
            if ((lolo & (1UL << (FixedMath.SHIFT_AMOUNT_I - 1))) != 0)
                loResult++; // Apply rounding up if the dropped bit is 1 (round half-up)

            bool overflow = false;
            long sum = FixedMath.AddOverflowHelper((long)loResult, lohi, ref overflow);
            sum = FixedMath.AddOverflowHelper(sum, hilo, ref overflow);
            sum = FixedMath.AddOverflowHelper(sum, hiResult, ref overflow);

            // Overflow handling
            bool opSignsEqual = ((xl ^ yl) & FixedMath.MIN_VALUE_L) == 0;

            // Positive overflow check
            if (opSignsEqual)
            {
                if (sum < 0 || (overflow && xl > 0))
                    return Fixed64.MAX_VALUE;
            }
            else
            {
                if (sum > 0)
                    return Fixed64.MIN_VALUE;
            }

            // Final overflow check: if the high 32 bits are non-zero or non-sign-extended, it's an overflow
            long topCarry = hihi >> FixedMath.SHIFT_AMOUNT_I;
            if (topCarry != 0 && topCarry != -1)
                return opSignsEqual ? Fixed64.MAX_VALUE : Fixed64.MIN_VALUE;

            // Negative overflow check
            if (!opSignsEqual)
            {
                long posOp = xl > yl ? xl : yl;
                long negOp = xl < yl ? xl : yl;

                if (sum > negOp && negOp < -FixedMath.ONE_L && posOp > FixedMath.ONE_L)
                    return Fixed64.MIN_VALUE;
            }

            return Fixed64.CreateRaw(sum);
        }
    }
}