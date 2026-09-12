using System;
using dcvietProcessor.Design.Beam.DTO;

namespace dcvietProcessor.Design.Beam.Crack.Calculator
{
    /// <summary>
    /// Tính ứng suất trong cốt thép chịu kéo
    /// của tiết diện đã nứt.
    ///
    /// Công thức:
    ///
    ///     sigmaS = M * (h0 - xm) * alphaS1 / Ired
    ///
    /// Trong đó:
    ///     h0     = h - a
    ///     xm     = chiều cao vùng nén
    ///     Ired   = mô men quán tính tiết diện quy đổi
    ///     alphaS1 = hệ số quy đổi mô đun đàn hồi
    ///
    /// Đơn vị:
    ///     M      : N.mm
    ///     h0, xm : mm
    ///     Ired   : mm4
    ///     sigmaS : N/mm2 = MPa
    /// </summary>
    public sealed class BeamCrackStressCalculator
    {
        private const double Tolerance = 1e-9;

        public double Calculate(
            double M,
            BeamDesignInput input,
            BeamEffectiveRebarPosition rebar,
            double xm,
            double Ired,
            double alphaS1)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (rebar == null)
                throw new ArgumentNullException(nameof(rebar));

            if (Ired <= Tolerance)
                return 0.0;

            if (alphaS1 <= Tolerance)
                return 0.0;

            double h = input.Height;

            double h0 =
                h - rebar.a;

            if (h0 <= Tolerance)
                return 0.0;

            double tensionLeverArm =
                h0 - xm;

            if (tensionLeverArm <= Tolerance)
                return 0.0;

            double sigmaS =
                M
                * tensionLeverArm
                * alphaS1
                / Ired;

            return Math.Abs(sigmaS);
        }
    }
}