using System;

namespace dcvietProcessor.Design.Beam.RebarSelection.BeamRebarLayout
{
    public sealed class RebarReinforcement
    {
        public int BarCount { get; }

        public double Diameter { get; }

        public double Area { get; }

        public RebarReinforcement(
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