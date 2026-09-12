using System;
using dcvietProcessor.Design.Beam.DTO;

namespace dcvietProcessor.Design.Beam.Crack.Calculator
{
    /// <summary>
    /// Tính mô men gây nứt của tiết diện dầm.
    ///
    /// Logic:
    ///     1. Tính tiết diện quy đổi Ared
    ///     2. Tính trọng tâm tiết diện quy đổi yt
    ///     3. Tính mô men quán tính tiết diện quy đổi Ired
    ///     4. Tính mô đun chống uốn Wred
    ///     5. Tính Mcrc
    ///
    /// Đơn vị:
    ///     Chiều dài : mm
    ///     Diện tích : mm2
    ///     Mô men    : N.mm
    /// </summary>
    public sealed class BeamCrackMcrcCalculator
    {
        private const double Tolerance = 1e-9;

        public double Calculate(
            BeamDesignInput input,
            BeamResolvedMaterialInput material,
            BeamEffectiveRebarPosition rebar)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (material == null)
                throw new ArgumentNullException(nameof(material));

            if (rebar == null)
                throw new ArgumentNullException(nameof(rebar));

            double b = input.Width;
            double h = input.Height;

            double Es = material.MainSteel.Es;
            double Eb = material.Concrete.Eb;
            double RbtSer = material.Concrete.RbtSer;

            if (b <= Tolerance ||
                h <= Tolerance ||
                Es <= Tolerance ||
                Eb <= Tolerance ||
                RbtSer <= Tolerance)
            {
                return 0.0;
            }

            // =====================================================
            // 1. Hệ số quy đổi cốt thép
            // =====================================================

            double alpha = Es / Eb;

            // =====================================================
            // 2. Diện tích tiết diện quy đổi
            //
            // Ared = b*h + α*As + α*As'
            // =====================================================

            double Ared =
                b * h
                + alpha * rebar.As
                + alpha * rebar.AsPrime;

            if (Ared <= Tolerance)
                return 0.0;

            // =====================================================
            // 3. Mô men tĩnh tiết diện quy đổi
            //
            // Stred =
            //     b*h²/2
            //     + α[
            //          As' (h-a')
            //          + As*a
            //       ]
            // =====================================================

            double Stred =
                b * h * h / 2.0
                + alpha
                * (
                    rebar.AsPrime * (h - rebar.aPrime)
                    + rebar.As * rebar.a
                  );

            // =====================================================
            // 4. Vị trí trọng tâm tiết diện quy đổi
            //
            // yt = Stred / Ared
            // =====================================================

            double yt = Stred / Ared;

            if (yt <= Tolerance)
                return 0.0;

            // =====================================================
            // 5. Mô men quán tính tiết diện quy đổi
            // đối với trục đi qua trọng tâm
            // =====================================================

            double Ired =
                b * Math.Pow(h, 3) / 12.0

                + alpha
                * rebar.As
                * Math.Pow(
                    yt - rebar.a,
                    2)

                + alpha
                * rebar.AsPrime
                * Math.Pow(
                    h - rebar.aPrime - yt,
                    2);

            if (Ired <= Tolerance)
                return 0.0;

            // =====================================================
            // 6. Mô đun chống uốn
            //
            // Wred = Ired / yt
            // =====================================================

            double Wred = Ired / yt;

            if (Wred <= Tolerance)
                return 0.0;

            // =====================================================
            // 7. Mô men gây nứt
            //
            // Mcrc = Rbt,ser * 1.3 * Wred
            // =====================================================

            return RbtSer * 1.3 * Wred;
        }
    }
}