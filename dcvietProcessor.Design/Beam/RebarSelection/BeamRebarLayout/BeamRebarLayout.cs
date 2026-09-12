using System;

namespace dcvietProcessor.Design.Beam.RebarSelection.BeamRebarLayout
{
    public sealed class BeamRebarLayout
    {
        public RebarSide Top { get; }

        public RebarSide Bottom { get; }

        public BeamRebarLayout(
            RebarSide top,
            RebarSide bottom)
        {
            if (top == null)
                throw new ArgumentNullException(
                    nameof(top));

            if (bottom == null)
                throw new ArgumentNullException(
                    nameof(bottom));

            Top = top;
            Bottom = bottom;
        }

        public double GetTopArea(
            RebarRegion region)
        {
            return Top.GetSectionArea(region);
        }

        public double GetBottomArea(
            RebarRegion region)
        {
            return Bottom.GetSectionArea(region);
        }
    }
}