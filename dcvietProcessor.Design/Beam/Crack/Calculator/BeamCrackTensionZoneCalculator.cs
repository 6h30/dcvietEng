using System;
using dcvietProcessor.Design.Beam.DTO;

namespace dcvietProcessor.Design.Beam.Crack.Calculator
{
    /// <summary>
    /// Xác định chiều cao vùng bê tông chịu kéo
    /// và diện tích bê tông chịu kéo dùng trong
    /// tính toán vết nứt.
    ///
    /// Logic:
    ///     1. Tính tiết diện quy đổi.
    ///     2. Tính trọng tâm tiết diện quy đổi.
    ///     3. Giới hạn yt theo điều kiện tính toán.
    ///     4. Tính Abt = b * yt.
    ///
    /// Đơn vị:
    ///     Chiều dài : mm
    ///     Diện tích : mm2
    /// </summary>
    public sealed class BeamCrackTensionZoneCalculator
    {
        private const double Tolerance = 1e-9;

        public double CalculateYt(
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

            if (b <= Tolerance ||
                h <= Tolerance)
                return 0.0;

            double Es =
                material.MainSteel.Es;

            double Eb =
                material.Concrete.Eb;

            if (Es <= Tolerance ||
                Eb <= Tolerance)
                return 0.0;

            // =====================================================
            // 1. Hệ số quy đổi
            // =====================================================

            double alpha =
                Es / Eb;

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
            // =====================================================

            double Stred =
                b * h * h / 2.0
                + alpha
                * (
                    rebar.AsPrime * (h - rebar.aPrime)
                    + rebar.As * rebar.a
                  );

            // =====================================================
            // 4. Trọng tâm tiết diện quy đổi
            // =====================================================

            double yt =
                Stred / Ared;

            if (yt <= Tolerance)
                return 0.0;

            // =====================================================
            // 5. Giới hạn chiều cao vùng chịu kéo
            //
            // yt <= 0.5h
            // yt >= 2a
            // =====================================================

            if (yt > 0.5 * h)
                yt = 0.5 * h;

            if (yt < 2.0 * rebar.a)
                yt = 2.0 * rebar.a;

            return yt;
        }

        public double CalculateAbt(
            BeamDesignInput input,
            double yt)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (yt <= Tolerance)
                return 0.0;

            return input.Width * yt;
        }
    }
}