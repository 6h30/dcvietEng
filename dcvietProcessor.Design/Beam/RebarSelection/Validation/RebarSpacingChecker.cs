using System;

namespace dcvietProcessor.Design.Beam.RebarSelection.Validation
{
    public sealed class RebarSpacingChecker
    {
        /// <summary>
        /// Kiểm tra một lớp thép có thể bố trí trong bề rộng dầm hay không.
        ///
        /// width       : bề rộng dầm, mm
        /// sideCover   : khoảng cách từ mép bê tông đến tim thanh ngoài cùng, mm
        /// barCount    : số thanh
        /// diameter    : đường kính thanh, mm
        /// minSpacing  : khoảng cách thông thủy nhỏ nhất giữa các thanh, mm
        /// công thức minSpacing cuối cùng chúng ta sẽ đưa về đúng quy định cấu tạo TCVN 5574:2018, thay vì để caller nhập tùy ý
        /// </summary>
        public bool CanFit(
            double width,
            double sideCover,
            int barCount,
            double diameter,
            double minSpacing)
        {
            if (width <= 0.0)
                return false;

            if (sideCover < 0.0)
                return false;

            if (barCount <= 0)
                return false;

            if (diameter <= 0.0)
                return false;

            if (minSpacing < 0.0)
                return false;

            if (barCount == 1)
            {
                return
                    2.0 * sideCover
                    + diameter
                    <= width + 1e-9;
            }

            double requiredWidth =
                2.0 * sideCover
                + barCount * diameter
                + (barCount - 1) * minSpacing;

            return requiredWidth
                   <= width + 1e-9;
        }
    }
}