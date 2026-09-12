using dcvietProcessor.Design.Beam.DTO;
using dcvietProcessor.Design.Beam.Flexure;
using dcvietProcessor.Design.Beam.RebarSelection.BeamRebarLayout;
using dcvietProcessor.Design.Beam.Shear;
using dcvietProcessor.Design.Beam.SteelGeometry;

namespace dcvietProcessor.Design.Beam
{
    /// <summary>
    /// Điều phối các phép tính thiết kế dầm.
    ///
    /// Engine chịu trách nhiệm:
    ///
    /// - xác định region đang tính;
    /// - xác định phía thép kéo theo dấu moment;
    /// - tính trọng tâm cốt thép theo layout;
    /// - cập nhật dữ liệu cần thiết vào BeamDesignInput;
    /// - gọi các calculator tương ứng.
    ///
    /// Calculator chỉ chịu trách nhiệm thực hiện công thức tính toán.
    /// </summary>
    public sealed class BeamDesignEngine
    {
        private readonly BeamFlexuralCapacityCalculator
            _flexuralCapacityCalculator;

        private readonly BeamShearCapacityCalculator
            _shearCapacityCalculator;

        private readonly BeamSteelCentroidCalculator
            _steelCentroidCalculator;

        public BeamDesignEngine()
        {
            _flexuralCapacityCalculator =
                new BeamFlexuralCapacityCalculator();

            _shearCapacityCalculator =
                new BeamShearCapacityCalculator();

            _steelCentroidCalculator =
                new BeamSteelCentroidCalculator();
        }

        // =========================================================
        // FLEXURE
        // =========================================================

        public BeamFlexureResult CalculateFlexuralCapacity(
            BeamDesignInput input,
            RebarRegion region,
            double moment,
            double concreteCover,
            params double[] layerSpacings)
        {
            return _flexuralCapacityCalculator.Calculate(
                input,
                region,
                moment,
                concreteCover,
                layerSpacings);
        }

        // =========================================================
        // SHEAR
        // =========================================================

        public double CalculateShearCapacity(
            BeamDesignInput input,
            RebarRegion region,
            double moment,
            double concreteCover,
            double Asw,
            double sw,
            params double[] layerSpacings)
        {
            // =====================================================
            // 1. XÁC ĐỊNH PHÍA THÉP KÉO
            //
            // Quy ước:
            //
            // M >= 0
            //     Bottom = tension
            //
            // M < 0
            //     Top = tension
            // =====================================================

            RebarSide tensileSide =
                GetTensileSide(
                    input,
                    moment);

            // =====================================================
            // 2. TÍNH TRỌNG TÂM THÉP KÉO
            // =====================================================

            double tensileSteelCentroid =
                _steelCentroidCalculator.Calculate(
                    tensileSide,
                    region,
                    concreteCover,
                    layerSpacings);

            // =====================================================
            // 3. CẬP NHẬT DTO
            //
            // Shear calculator chỉ lấy dữ liệu từ BeamDesignInput.
            // =====================================================

            input.TensileSteelCentroid =
                tensileSteelCentroid;

            // =====================================================
            // 4. SHEAR CAPACITY
            // =====================================================

            return _shearCapacityCalculator.Calculate(
                input,
                Asw,
                sw);
        }

        // =========================================================
        // MAXIMUM STIRRUP SPACING
        // =========================================================

        public double CalculateMaximumStirrupSpacing(
            BeamDesignInput input,
            RebarRegion region,
            double moment,
            double concreteCover,
            double shearForce,
            params double[] layerSpacings)
        {
            // =====================================================
            // 1. XÁC ĐỊNH PHÍA THÉP KÉO
            // =====================================================

            RebarSide tensileSide =
                GetTensileSide(
                    input,
                    moment);

            // =====================================================
            // 2. TÍNH TRỌNG TÂM THÉP KÉO
            // =====================================================

            double tensileSteelCentroid =
                _steelCentroidCalculator.Calculate(
                    tensileSide,
                    region,
                    concreteCover,
                    layerSpacings);

            // =====================================================
            // 3. CẬP NHẬT DTO
            // =====================================================

            input.TensileSteelCentroid =
                tensileSteelCentroid;

            // =====================================================
            // 4. SW MAX
            // =====================================================

            return _shearCapacityCalculator
                .CalculateMaximumStirrupSpacing(
                    input,
                    shearForce);
        }

        // =========================================================
        // TENSILE SIDE
        // =========================================================

        private static RebarSide GetTensileSide(
            BeamDesignInput input,
            double moment)
        {
            if (moment >= 0.0)
            {
                return input.RebarLayout.Bottom;
            }

            return input.RebarLayout.Top;
        }
    }
}