using System;

namespace dcvietProcessor.Design.Beam.RebarSelection.Models
{
    /// <summary>
    /// Một phương án cốt thép ứng viên.
    /// Candidate chỉ mô tả số thanh, đường kính và diện tích thép.
    /// </summary>
    public sealed class RebarCandidate
    {
        /// <summary>
        /// Số lượng thanh.
        /// </summary>
        public int BarCount { get; }

        /// <summary>
        /// Đường kính thanh thép, mm.
        /// </summary>
        public double Diameter { get; }

        /// <summary>
        /// Tổng diện tích cốt thép, mm2.
        /// </summary>
        public double Area { get; }

        public RebarCandidate(
            int barCount,
            double diameter)
        {
            if (barCount <= 0)
                throw new ArgumentException(
                    "BarCount must be > 0.");

            if (diameter <= 0.0)
                throw new ArgumentException(
                    "Diameter must be > 0.");

            BarCount = barCount;
            Diameter = diameter;

            Area =
                barCount
                * Math.PI
                * diameter
                * diameter
                / 4.0;
        }

        public override string ToString()
        {
            return string.Format(
                "{0}D{1:0}",
                BarCount,
                Diameter);
        }
    }
}