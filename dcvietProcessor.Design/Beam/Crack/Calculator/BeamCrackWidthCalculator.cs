using System;
using dcvietProcessor.Design.Beam.Crack.Calculator;
using dcvietProcessor.Design.Beam.DTO;

namespace dcvietProcessor.Design.Beam.Crack
{
    /// <summary>
    /// Tính bề rộng khe nứt cho một trường hợp nội lực.
    ///
    /// Calculator này tương đương một lần gọi acrci()
    /// trong VBA.
    ///
    /// Không chịu trách nhiệm:
    /// - tính Crack1 / Crack2 / Crack3;
    /// - tổ hợp ShortTerm / LongTerm;
    /// - chọn cốt thép;
    /// - kiểm tra đạt / không đạt.
    ///
    /// Quy ước:
    ///     isLS = 1 : ngắn hạn
    ///     isLS = 2 : dài hạn
    /// </summary>
    public sealed class BeamCrackWidthCalculator
    {
        private const double Tolerance = 1e-9;

        private readonly BeamCrackMcrcCalculator _mcrcCalculator;
        private readonly BeamCrackNeutralAxisCalculator _neutralAxisCalculator;
        private readonly BeamCrackMomentOfInertiaCalculator _inertiaCalculator;
        private readonly BeamCrackStressCalculator _stressCalculator;
        private readonly BeamCrackTensionZoneCalculator _tensionZoneCalculator;
        private readonly BeamCrackSpacingCalculator _spacingCalculator;

        public BeamCrackWidthCalculator(
            BeamCrackMcrcCalculator mcrcCalculator,
            BeamCrackNeutralAxisCalculator neutralAxisCalculator,
            BeamCrackMomentOfInertiaCalculator inertiaCalculator,
            BeamCrackStressCalculator stressCalculator,
            BeamCrackTensionZoneCalculator tensionZoneCalculator,
            BeamCrackSpacingCalculator spacingCalculator)
        {
            _mcrcCalculator =
                mcrcCalculator
                ?? throw new ArgumentNullException(nameof(mcrcCalculator));

            _neutralAxisCalculator =
                neutralAxisCalculator
                ?? throw new ArgumentNullException(nameof(neutralAxisCalculator));

            _inertiaCalculator =
                inertiaCalculator
                ?? throw new ArgumentNullException(nameof(inertiaCalculator));

            _stressCalculator =
                stressCalculator
                ?? throw new ArgumentNullException(nameof(stressCalculator));

            _tensionZoneCalculator =
                tensionZoneCalculator
                ?? throw new ArgumentNullException(nameof(tensionZoneCalculator));

            _spacingCalculator =
                spacingCalculator
                ?? throw new ArgumentNullException(nameof(spacingCalculator));
        }

        /// <summary>
        /// Tính bề rộng khe nứt cho một lần gọi acrci().
        ///
        /// M:
        ///     Mô men tính toán.
        ///
        /// Mh:
        ///     Mô men dùng để xác định trạng thái nứt
        ///     và hệ số ψs.
        ///
        /// isLS:
        ///     1 = ngắn hạn
        ///     2 = dài hạn
        ///
        /// representativeBarDiameter:
        ///     Đường kính thanh cốt thép đại diện ds.
        /// </summary>
        public double Calculate(
            BeamDesignInput input,
            BeamEffectiveRebarPosition rebar,
            double M,
            double Mh,
            int isLS,
            double representativeBarDiameter)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (rebar == null)
                throw new ArgumentNullException(nameof(rebar));

            if (isLS != 1 && isLS != 2)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(isLS),
                    "isLS phải bằng 1 (ngắn hạn) hoặc 2 (dài hạn).");
            }

            // =====================================================
            // 0. NỘI LỰC VÀ DỮ LIỆU CƠ BẢN
            // =====================================================

            M = Math.Abs(M);
            Mh = Math.Abs(Mh);

            if (M <= Tolerance)
                return 0.0;

            if (Mh <= Tolerance)
                return 0.0;

            if (rebar.As <= Tolerance)
                return 0.0;

            if (representativeBarDiameter <= Tolerance)
                return 0.0;

            // =====================================================
            // 1. Mcrc
            // =====================================================

            double Mcrc =
                _mcrcCalculator.Calculate(
                    input,
                    input.Material,
                    rebar);

            if (Mcrc <= Tolerance)
                return 0.0;

            // =====================================================
            // 2. ψs
            // =====================================================

            double psiS;

            if (0.8 * Mcrc < Mh)
            {
                psiS =
                    1.0
                    - 0.8 * Mcrc / Mh;
            }
            else
            {
                // Bám đúng logic VBA:
                //
                // posi_s = 0.000001
                //
                psiS = 0.000001;
            }

            // =====================================================
            // 3. HỆ SỐ φ1 VÀ εb1,red
            // =====================================================

            double phi1;
            double epsilonB1Red;

            if (isLS == 1)
            {
                phi1 = 1.0;
                epsilonB1Red = 0.0015;
            }
            else
            {
                phi1 = 1.4;
                epsilonB1Red = 0.0024;
            }

            // =====================================================
            // 4. HỆ SỐ φ2
            // =====================================================

            double phi2 =
                representativeBarDiameter >= 10.0
                    ? 0.5
                    : 0.8;

            // =====================================================
            // 5. MÔ ĐUN QUY ĐỔI
            // =====================================================

            double Es =
                input.Material.MainSteel.Es;

            double RbSer =
                input.Material.Concrete.RbSer;

            if (Es <= Tolerance)
                return 0.0;

            if (RbSer <= Tolerance)
                return 0.0;

            double EbRed =
                RbSer / epsilonB1Red;

            if (EbRed <= Tolerance)
                return 0.0;

            // -----------------------------------------------------
            // Bám đúng VBA:
            //
            // E_sred = E_s / posi_s
            // alpha_s1 = E_s / E_bred
            // alpha_s2 = alpha_s1
            //
            // E_sred thực tế không được sử dụng tiếp trong VBA.
            // -----------------------------------------------------

            double EsRed =
                Es / psiS;

            // Giữ biến để thể hiện đúng logic acrci().
            // Không sử dụng EsRed trong công thức tiếp theo.
            _ = EsRed;

            double alphaS1 =
                Es / EbRed;

            double alphaS2 =
                alphaS1;

            // =====================================================
            // 6. TRỤC TRUNG HÒA xm
            // =====================================================

            double xm =
                _neutralAxisCalculator.Calculate(
                    input,
                    rebar,
                    alphaS1,
                    alphaS2);

            if (xm <= Tolerance)
                return 0.0;

            // =====================================================
            // 7. MOMENT QUÁN TÍNH Ired
            // =====================================================

            double Ired =
                _inertiaCalculator.Calculate(
                    input,
                    rebar,
                    xm,
                    alphaS1,
                    alphaS2);

            if (Ired <= Tolerance)
                return 0.0;

            // =====================================================
            // 8. ỨNG SUẤT CỐT THÉP σs
            // =====================================================

            double sigmaS =
                _stressCalculator.Calculate(
                    M,
                    input,
                    rebar,
                    xm,
                    Ired,
                    alphaS1);

            sigmaS =
                Math.Abs(sigmaS);

            if (sigmaS <= Tolerance)
                return 0.0;

            // =====================================================
            // 9. VÙNG BÊ TÔNG CHỊU KÉO
            // =====================================================

            double yt =
                _tensionZoneCalculator.CalculateYt(
                    input,
                    input.Material,
                    rebar);

            if (yt <= Tolerance)
                return 0.0;

            double Abt =
                _tensionZoneCalculator.CalculateAbt(
                    input,
                    yt);

            if (Abt <= Tolerance)
                return 0.0;

            // =====================================================
            // 10. KHOẢNG CÁCH KHE NỨT Ls
            // =====================================================

            double Ls =
                _spacingCalculator.Calculate(
                    Abt,
                    representativeBarDiameter,
                    rebar.As);

            if (Ls <= Tolerance)
                return 0.0;

            // =====================================================
            // 11. BỀ RỘNG KHE NỨT acrc
            // =====================================================

            const double phi3 = 1.0;

            double acrc =
                phi1
                * phi2
                * phi3
                * psiS
                * sigmaS
                * Ls
                / Es;

            return Math.Max(
                0.0,
                acrc);
        }
    }
}