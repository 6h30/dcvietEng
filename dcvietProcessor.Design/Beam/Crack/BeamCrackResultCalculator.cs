using System;
using dcvietProcessor.Design.Beam.DTO;

namespace dcvietProcessor.Design.Beam.Crack
{
    /// <summary>
    /// Tính kết quả nứt tại MỘT vị trí của dầm.
    ///
    /// Một lần Calculate() tương ứng với một vị trí
    /// cụ thể của dầm, ví dụ:
    ///     Start
    ///     Middle
    ///     End
    ///
    /// Tại vị trí đó, calculator thực hiện 3 lần acrci():
    ///
    ///     Crack1 = acrci(|M3|, ..., isLS=2, |Mh2|)
    ///     Crack2 = acrci(|M1|, ..., isLS=1, |Mh1|)
    ///     Crack3 = acrci(|M2|, ..., isLS=1, |Mh1|)
    ///
    /// Sau đó:
    ///
    ///     ShortTerm = Crack1 + Crack2 - Crack3
    ///     LongTerm  = Crack1
    ///
    /// Class này KHÔNG:
    /// - gộp Start / Middle / End;
    /// - chọn giá trị lớn nhất giữa các vị trí;
    /// - kiểm tra đạt / không đạt.
    /// </summary>
    public sealed class BeamCrackResultCalculator
    {
        private const double Tolerance = 1e-9;

        private readonly BeamCrackWidthCalculator _calculator;

        public BeamCrackResultCalculator(
            BeamCrackWidthCalculator calculator)
        {
            _calculator =
                calculator
                ?? throw new ArgumentNullException(nameof(calculator));
        }

        /// <summary>
        /// Tính kết quả nứt tại MỘT vị trí của dầm.
        ///
        /// force chứa toàn bộ nội lực của đúng vị trí đang xét.
        ///
        /// Không được truyền lực của nhiều vị trí vào cùng một lần Calculate().
        /// </summary>
        public BeamCrackResult Calculate(
            BeamDesignInput input,
            BeamDesignForceInput force,
            BeamEffectiveRebarPosition rebar,
            double representativeBarDiameter)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (force == null)
                throw new ArgumentNullException(nameof(force));

            if (rebar == null)
                throw new ArgumentNullException(nameof(rebar));

            if (representativeBarDiameter <= Tolerance)
                return new BeamCrackResult();

            // =========================================================
            // CRACK 1
            //
            // VBA:
            //
            // a_crc1 = acrci(
            //     Abs(M3),
            //     ...,
            //     2,
            //     rbt,
            //     Abs(Mh2))
            //
            // isLS = 2 -> DÀI HẠN
            // =========================================================

            double crack1 =
                _calculator.Calculate(
                    input,
                    rebar,
                    Math.Abs(force.M3),
                    Math.Abs(force.Mh2),
                    2,
                    representativeBarDiameter);

            // =========================================================
            // CRACK 2
            //
            // VBA:
            //
            // a_crc2 = acrci(
            //     Abs(M1),
            //     ...,
            //     1,
            //     rbt,
            //     Abs(Mh1))
            //
            // isLS = 1 -> NGẮN HẠN
            // =========================================================

            double crack2 =
                _calculator.Calculate(
                    input,
                    rebar,
                    Math.Abs(force.M1),
                    Math.Abs(force.Mh1),
                    1,
                    representativeBarDiameter);

            // =========================================================
            // CRACK 3
            //
            // VBA:
            //
            // a_crc3 = acrci(
            //     Abs(M2),
            //     ...,
            //     1,
            //     rbt,
            //     Abs(Mh1))
            //
            // isLS = 1 -> NGẮN HẠN
            // =========================================================

            double crack3 =
                _calculator.Calculate(
                    input,
                    rebar,
                    Math.Abs(force.M2),
                    Math.Abs(force.Mh1),
                    1,
                    representativeBarDiameter);

            // =========================================================
            // SHORT TERM
            //
            // VBA:
            //
            // a_crc_NganHan = a_crc1 + a_crc2 - a_crc3
            // =========================================================

            double shortTerm =
                crack1
                + crack2
                - crack3;

            // =========================================================
            // LONG TERM
            //
            // VBA:
            //
            // a_crc_DaiHan = a_crc1
            // =========================================================

            double longTerm =
                crack1;

            // =========================================================
            // RESULT
            // =========================================================

            return new BeamCrackResult
            {
                Crack1 = Math.Round(crack1, 3),
                Crack2 = Math.Round(crack2, 3),
                Crack3 = Math.Round(crack3, 3),

                ShortTerm = Math.Round(shortTerm, 3),
                LongTerm = Math.Round(longTerm, 3)
            };
        }
    }
}