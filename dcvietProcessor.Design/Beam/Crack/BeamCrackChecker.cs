using System;

namespace dcvietProcessor.Design.Beam.Crack
{
    /// <summary>
    /// Kiểm tra điều kiện khe nứt của dầm
    /// tại ba vị trí:
    ///
    ///     Start
    ///     Middle
    ///     End
    ///
    /// Tương đương logic VBA CheckNut().
    ///
    /// Class này không thực hiện tính toán khe nứt.
    /// Nó chỉ lấy giá trị lớn nhất tại ba vị trí
    /// và so sánh với giới hạn cho phép.
    /// </summary>
    public sealed class BeamCrackChecker
    {
        public string Check(
            BeamCrackResult start,
            BeamCrackResult middle,
            BeamCrackResult end,
            double limitShort,
            double limitLong)
        {
            if (start == null)
                throw new ArgumentNullException(nameof(start));

            if (middle == null)
                throw new ArgumentNullException(nameof(middle));

            if (end == null)
                throw new ArgumentNullException(nameof(end));

            if (limitShort < 0.0)
                throw new ArgumentOutOfRangeException(
                    nameof(limitShort),
                    "Giới hạn khe nứt ngắn hạn không được âm.");

            if (limitLong < 0.0)
                throw new ArgumentOutOfRangeException(
                    nameof(limitLong),
                    "Giới hạn khe nứt dài hạn không được âm.");

            // =====================================================
            // MAX SHORT TERM
            //
            // VBA:
            //
            // Nut_Max_Short =
            //     Application.Max(
            //         Nut_Trai_Short,
            //         Nut_Giua_Short,
            //         Nut_Phai_Short)
            // =====================================================

            double maxShort =
                Math.Max(
                    start.ShortTerm,
                    Math.Max(
                        middle.ShortTerm,
                        end.ShortTerm));

            // =====================================================
            // MAX LONG TERM
            //
            // VBA:
            //
            // Nut_Max_Long =
            //     Application.Max(
            //         Nut_Trai_Long,
            //         Nut_Giua_Long,
            //         Nut_Phai_Long)
            // =====================================================

            double maxLong =
                Math.Max(
                    start.LongTerm,
                    Math.Max(
                        middle.LongTerm,
                        end.LongTerm));

            // =====================================================
            // CHECK
            //
            // VBA:
            //
            // If Nut_Max_Short <= CDL_Short
            // And Nut_Max_Long <= CDL_Long Then
            //     CheckNut = "OK"
            // Else
            //     CheckNut = "NOT"
            // End If
            // =====================================================

            if (maxShort <= limitShort &&
                maxLong <= limitLong)
            {
                return "OK";
            }

            return "NOT";
        }
    }
}