using System;

namespace dcvietProcessor.Design.Beam.Crack.Calculator
{
    /// <summary>
    /// Tính khoảng cách trung bình giữa các vết nứt Ls
    /// trong vùng chịu kéo.
    ///
    /// Công thức hiện tại bám theo logic VBA acrci():
    ///
    ///     Ls = 0.5 * Abt * ds / As
    ///
    /// với giới hạn:
    ///
    ///     Ls >= Max(10*ds, 100)
    ///     Ls <= Min(40*ds, 400)
    ///
    /// Đơn vị:
    ///     Abt : mm2
    ///     ds  : mm
    ///     As  : mm2
    ///     Ls  : mm
    /// </summary>
    public sealed class BeamCrackSpacingCalculator
    {
        private const double Tolerance = 1e-9;

        public double Calculate(
            double Abt,
            double representativeBarDiameter,
            double As)
        {
            if (Abt <= Tolerance)
                return 0.0;

            if (representativeBarDiameter <= Tolerance)
                return 0.0;

            if (As <= Tolerance)
                return 0.0;

            double Ls =
                0.5
                * Abt
                * representativeBarDiameter
                / As;

            double minLs =
                Math.Max(
                    10.0 * representativeBarDiameter,
                    100.0);

            double maxLs =
                Math.Min(
                    40.0 * representativeBarDiameter,
                    400.0);

            if (Ls < minLs)
                Ls = minLs;

            if (Ls > maxLs)
                Ls = maxLs;

            return Ls;
        }
    }
}