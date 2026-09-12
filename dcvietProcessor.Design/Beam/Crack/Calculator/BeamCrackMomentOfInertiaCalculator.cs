using System;
using dcvietProcessor.Design.Beam.DTO;

namespace dcvietProcessor.Design.Beam.Crack.Calculator
{
    /// <summary>
    /// Tính mô men quán tính của tiết diện quy đổi
    /// trong trạng thái tiết diện đã hình thành vùng nứt.
    ///
    /// Thành phần:
    ///     I_b  : phần bê tông vùng nén
    ///     I_s  : phần cốt thép chịu kéo quy đổi
    ///     I_sp : phần cốt thép chịu nén quy đổi
    ///
    /// I_red = I_b + αs2 * I_s + αs1 * I_sp
    ///
    /// Đơn vị:
    ///     Chiều dài : mm
    ///     Diện tích : mm2
    ///     I_red     : mm4
    /// </summary>
    public sealed class BeamCrackMomentOfInertiaCalculator
    {
        private const double Tolerance = 1e-9;

        public double Calculate(
            BeamDesignInput input,
            BeamEffectiveRebarPosition rebar,
            double xm,
            double alphaS1,
            double alphaS2)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (rebar == null)
                throw new ArgumentNullException(nameof(rebar));

            if (xm <= Tolerance)
                return 0.0;

            if (alphaS1 <= Tolerance ||
                alphaS2 <= Tolerance)
                return 0.0;

            double b = input.Width;
            double h = input.Height;

            if (b <= Tolerance ||
                h <= Tolerance)
                return 0.0;

            // =====================================================
            // 1. Chiều cao làm việc
            //
            // h0 = h - a
            // =====================================================

            double h0 =
                h - rebar.a;

            if (h0 <= Tolerance)
                return 0.0;

            // =====================================================
            // 2. Cánh tay đòn phần thép kéo
            //
            // h0 - xm
            // =====================================================

            double tensionLeverArm =
                h0 - xm;

            if (tensionLeverArm <= Tolerance)
                return 0.0;

            // =====================================================
            // 3. Cánh tay đòn phần thép nén
            //
            // xm - a'
            // =====================================================

            double compressionLeverArm =
                xm - rebar.aPrime;

            if (compressionLeverArm < 0.0)
                return 0.0;

            // =====================================================
            // 4. Mô men quán tính phần bê tông vùng nén
            //
            // I_b = b*xm^3/3
            //
            // Tương đương VBA:
            //
            // b*xm^3/12 + b*xm*xm^2/4
            // =====================================================

            double concrete =
                b * Math.Pow(xm, 3) / 3.0;

            // =====================================================
            // 5. Mô men quán tính cốt thép kéo
            //
            // I_s = As * (h0 - xm)^2
            // =====================================================

            double tensionSteel =
                rebar.As
                * Math.Pow(tensionLeverArm, 2)
                * alphaS2;

            // =====================================================
            // 6. Mô men quán tính cốt thép nén
            //
            // I_sp = As' * (xm - a')^2
            // =====================================================

            double compressionSteel =
                rebar.AsPrime
                * Math.Pow(compressionLeverArm, 2)
                * alphaS1;

            // =====================================================
            // 7. Mô men quán tính tiết diện quy đổi
            //
            // Ired = Ib + αs2*Is + αs1*Isp
            // =====================================================

            double Ired =
                concrete
                + tensionSteel
                + compressionSteel;

            if (Ired <= Tolerance)
                return 0.0;

            return Ired;
        }
    }
}