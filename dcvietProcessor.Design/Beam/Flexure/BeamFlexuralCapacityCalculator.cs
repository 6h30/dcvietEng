using System;

using dcvietProcessor.Design.Beam.DTO;
using dcvietProcessor.Design.Beam.RebarSelection.BeamRebarLayout;
using dcvietProcessor.Design.Beam.SteelGeometry;

namespace dcvietProcessor.Design.Beam.Flexure
{
    /// <summary>
    /// Tính khả năng chịu uốn của tiết diện dầm.
    ///
    /// Port từ VBA:
    ///     KhaNangChiuUon()
    ///
    /// TOP / BOTTOM:
    ///     vị trí hình học của cốt thép.
    ///
    /// TENSION / COMPRESSION:
    ///     được xác định theo dấu moment
    ///     tại region đang kiểm tra.
    ///
    /// Trọng tâm cốt thép được tính trực tiếp
    /// từ BeamRebarLayout.
    /// </summary>
    public sealed class BeamFlexuralCapacityCalculator
    {
        private const double EpsilonB2 = 0.0035;
        private const double XiCoefficient = 0.8;

        private readonly BeamSteelCentroidCalculator
            _centroidCalculator;

        public BeamFlexuralCapacityCalculator()
        {
            _centroidCalculator =
                new BeamSteelCentroidCalculator();
        }

        // =========================================================
        // CALCULATE
        // =========================================================

        public BeamFlexureResult Calculate(
            BeamDesignInput input,
            RebarRegion region,
            double moment,
            double concreteCover,
            params double[] layerSpacings)
        {
            if (input == null)
            {
                throw new ArgumentNullException(
                    nameof(input));
            }

            if (input.RebarLayout == null)
            {
                throw new ArgumentNullException(
                    nameof(input.RebarLayout));
            }

            // =====================================================
            // 1. GEOMETRY
            // =====================================================

            double b =
                input.Width;

            double h =
                input.Height;

            // =====================================================
            // 2. STEEL CENTROID
            //
            // TOP:
            //
            //     Continuous + region
            //
            // BOTTOM:
            //
            //     Continuous + region
            //
            // Ví dụ:
            //
            // region = Left
            //
            // Top:
            //     Continuous + Left
            //
            // Bottom:
            //     Continuous + Left
            // =====================================================

            double topSteelCentroid =
                _centroidCalculator.Calculate(
                    input.RebarLayout.Top,
                    region,
                    concreteCover,
                    layerSpacings);

            double bottomSteelCentroid =
                _centroidCalculator.Calculate(
                    input.RebarLayout.Bottom,
                    region,
                    concreteCover,
                    layerSpacings);

            // =====================================================
            // 3. XÁC ĐỊNH THÉP KÉO / NÉN
            //
            // Quy ước:
            //
            // M >= 0
            //
            //     Bottom = tension
            //     Top    = compression
            //
            // M < 0
            //
            //     Top    = tension
            //     Bottom = compression
            // =====================================================

            bool positiveMoment =
                moment >= 0.0;

            double As;
            double AsPrime;

            double a;
            double aPrime;

            if (positiveMoment)
            {
                // -------------------------------------------------
                // Moment dương
                //
                // Bottom kéo
                // Top nén
                // -------------------------------------------------

                As =
                    input.RebarLayout
                        .Bottom
                        .GetSectionArea(region);

                AsPrime =
                    input.RebarLayout
                        .Top
                        .GetSectionArea(region);

                a =
                    bottomSteelCentroid;

                aPrime =
                    topSteelCentroid;
            }
            else
            {
                // -------------------------------------------------
                // Moment âm
                //
                // Top kéo
                // Bottom nén
                // -------------------------------------------------

                As =
                    input.RebarLayout
                        .Top
                        .GetSectionArea(region);

                AsPrime =
                    input.RebarLayout
                        .Bottom
                        .GetSectionArea(region);

                a =
                    topSteelCentroid;

                aPrime =
                    bottomSteelCentroid;
            }

            // =====================================================
            // 4. MATERIAL
            // =====================================================

            double Rb =
                input.Material.Concrete.Rb;

            double Rs =
                input.Material.MainSteel.Rs;

            double Rsc =
                input.Material.MainSteel.Rsc;

            double Es =
                input.Material.MainSteel.Es;

            // =====================================================
            // VBA:
            //
            // Epsilon_b2 = 0.0035
            //
            // Epsilon_sel =
            //     R_s / E_s
            //
            // Xi_R =
            //     0.8 /
            //     (1 + Epsilon_sel / Epsilon_b2)
            // =====================================================

            double epsilonSel =
                Rs / Es;

            double xiR =
                XiCoefficient
                /
                (
                    1.0
                    + epsilonSel
                    / EpsilonB2
                );

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
            // x =
            //
            //     (R_s * A_s - R_sc * A_sp)
            //     /
            //     (R_b * b)
            // =====================================================

            double xRaw =
                (
                    Rs * As
                    - Rsc * AsPrime
                )
                /
                (Rb * b);

            // =====================================================
            // VBA:
            //
            // If x >= Xi_R * h_0 Then
            //
            //     x = Xi_R * h_0
            //
            // End If
            // =====================================================

            double xLimit =
                xiR * h0;

            double x =
                xRaw;

            if (x >= xLimit)
            {
                x =
                    xLimit;
            }

            // =====================================================
            // VBA:
            //
            // M_u =
            //
            //     R_b * b * x
            //     * (h_0 - 0.5 * x)
            //
            //     +
            //
            //     R_sc * A_sp
            //     * (h_0 - a_p)
            // =====================================================

            double momentCapacity =
                Rb
                * b
                * x
                * (h0 - 0.5 * x)
                +
                Rsc
                * AsPrime
                * (h0 - aPrime);

            // =====================================================
            // VBA:
            //
            // If R_sc * A_sp >= R_s * A_s Then
            //
            //     M_u =
            //
            //         R_s * A_s
            //         * (h_0 - a_p)
            //
            // End If
            // =====================================================

            bool compressionSteelControls =
                Rsc * AsPrime
                >=
                Rs * As;

            if (compressionSteelControls)
            {
                momentCapacity =
                    Rs
                    * As
                    * (h0 - aPrime);
            }

            // =====================================================
            // 5. Xi
            // =====================================================

            double xi =
                x / h0;

            // =====================================================
            // 6. RESULT
            // =====================================================

            return new BeamFlexureResult
            {
                EpsilonSel =
                    epsilonSel,

                EpsilonB2 =
                    EpsilonB2,

                XiR =
                    xiR,

                Xi =
                    xi,

                H0 =
                    h0,

                XRaw =
                    xRaw,

                X =
                    x,

                XLimit =
                    xLimit,

                MomentCapacity =
                    momentCapacity,

                CompressionSteelControls =
                    compressionSteelControls
            };
        }
    }
}