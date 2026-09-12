using System;

using dcvietProcessor.Design.Beam.DTO;

namespace dcvietProcessor.Design.Beam.Shear
{
    /// <summary>
    /// Tính khả năng chịu cắt của tiết diện dầm.
    ///
    /// Port từ VBA:
    ///
    ///     KhaNangChiuCat()
    ///     swmax()
    ///
    /// Calculator chỉ chịu trách nhiệm tính toán.
    ///
    /// Dữ liệu hình học, vật liệu và trọng tâm thép
    /// được lấy từ BeamDesignInput.
    ///
    /// Cốt đai của lần kiểm tra được truyền vào:
    ///
    ///     Asw : diện tích cốt đai, mm2
    ///     sw  : bước cốt đai, mm
    ///
    /// Đơn vị:
    ///
    ///     b, h, a, sw : mm
    ///     Rb, Rbt, Rsw : MPa = N/mm2
    ///     Asw : mm2
    ///     Q   : N
    /// </summary>
    public sealed class BeamShearCapacityCalculator
    {
        private const double PhiB1 = 0.3;
        private const double PhiB2 = 1.5;
        private const double PhiSw = 0.75;

        // =========================================================
        // SHEAR CAPACITY
        // =========================================================

        public double Calculate(
            BeamDesignInput input,
            double Asw,
            double sw)
        {
            // =====================================================
            // INPUT
            // =====================================================

            double b =
                input.Width;

            double h =
                input.Height;

            double a =
                input.TensileSteelCentroid;

            // =====================================================
            // MATERIAL
            //
            // Material đã được resolve trước khi truyền
            // BeamDesignInput vào calculator.
            // =====================================================

            double Rb =
                input.Material.Concrete.Rb;

            double Rbt =
                input.Material.Concrete.Rbt;

            double Rsw =
                input.Material.StirrupSteel.Rsw;

            // =====================================================
            // VBA:
            //
            // h_0 = h - a
            // =====================================================

            double h0 =
                h - a;

            // =====================================================
            // VBA:
            //
            // phi_b1 = 0.3
            //
            // Q_giuadai =
            //     phi_b1 * R_b * b * h_0
            // =====================================================

            double qGiuadai =
                PhiB1
                * Rb
                * b
                * h0;

            // =====================================================
            // VBA:
            //
            // q_sw =
            //     R_sw * A_sw / s_w
            // =====================================================

            double qSw =
                Rsw
                * Asw
                / sw;

            // =====================================================
            // VBA:
            //
            // If q_sw < 0.25 * R_bt * b Then
            //
            //     q_sw = 0.1
            //
            // End If
            //
            // Giữ nguyên logic VBA.
            // =====================================================

            if (qSw < 0.25 * Rbt * b)
            {
                qSw =
                    0.1;
            }

            // =====================================================
            // VBA:
            //
            // C =
            //     Sqr(
            //         phi_b2
            //         * R_bt
            //         * b
            //         * h_0 ^ 2
            //         /
            //         (phi_sw * q_sw)
            //     )
            // =====================================================

            double c =
                Math.Sqrt(
                    PhiB2
                    * Rbt
                    * b
                    * h0
                    * h0
                    /
                    (PhiSw * qSw));

            // =====================================================
            // VBA:
            //
            // If C < h_0 Then
            //     C = h_0
            // End If
            //
            // If C > 2 * h_0 Then
            //     C = 2 * h_0
            // End If
            // =====================================================

            if (c < h0)
            {
                c =
                    h0;
            }

            if (c > 2.0 * h0)
            {
                c =
                    2.0 * h0;
            }

            // =====================================================
            // VBA:
            //
            // Q_b =
            //     phi_b2
            //     * R_bt
            //     * b
            //     * h_0 ^ 2
            //     / C
            // =====================================================

            double qb =
                PhiB2
                * Rbt
                * b
                * h0
                * h0
                / c;

            // =====================================================
            // VBA:
            //
            // If Q_b < 0.5 * R_bt * b * h_0 Then
            //
            //     Q_b =
            //         0.5 * R_bt * b * h_0
            //
            // End If
            // =====================================================

            double qbMin =
                0.5
                * Rbt
                * b
                * h0;

            if (qb < qbMin)
            {
                qb =
                    qbMin;
            }

            // =====================================================
            // VBA:
            //
            // If Q_b > 2.5 * R_bt * b * h_0 Then
            //
            //     Q_b =
            //         2.5 * R_bt * b * h_0
            //
            // End If
            // =====================================================

            double qbMax =
                2.5
                * Rbt
                * b
                * h0;

            if (qb > qbMax)
            {
                qb =
                    qbMax;
            }

            // =====================================================
            // VBA:
            //
            // Qsw =
            //     phi_sw * q_sw * C
            // =====================================================

            double qsw =
                PhiSw
                * qSw
                * c;

            // =====================================================
            // VBA:
            //
            // KhaNangChiuCat =
            //
            //     Application.Min(
            //         Q_giuadai,
            //         Q_b + Qsw
            //     )
            // =====================================================

            double shearCapacity =
                Math.Min(
                    qGiuadai,
                    qb + qsw);

            return shearCapacity;
        }

        // =========================================================
        // MAXIMUM STIRRUP SPACING
        // =========================================================

        /// <summary>
        /// Tính bước cốt đai lớn nhất.
        ///
        /// Port từ VBA:
        ///
        ///     swmax()
        ///
        /// VBA:
        ///
        ///     swmax =
        ///         R_bt * b * h_0 ^ 2 / Q
        /// </summary>
        public double CalculateMaximumStirrupSpacing(
            BeamDesignInput input,
            double shearForce)
        {
            // =====================================================
            // INPUT
            // =====================================================

            double b =
                input.Width;

            double h =
                input.Height;

            double a =
                input.TensileSteelCentroid;

            double Rbt =
                input.Material.Concrete.Rbt;

            // =====================================================
            // VBA:
            //
            // h_0 = h - a
            // =====================================================

            double h0 =
                h - a;

            // =====================================================
            // VBA:
            //
            // swmax =
            //     R_bt * b * h_0 ^ 2 / Q
            // =====================================================

            double maximumSpacing =
                Rbt
                * b
                * h0
                * h0
                / shearForce;

            return maximumSpacing;
        }
    }
}