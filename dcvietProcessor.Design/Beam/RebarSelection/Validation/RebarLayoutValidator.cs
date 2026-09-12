using System;
using dcvietProcessor.Design.Beam.RebarSelection.BeamRebarLayout;

using BeamRebarLayoutModel =
    dcvietProcessor.Design.Beam.RebarSelection.BeamRebarLayout.BeamRebarLayout;

namespace dcvietProcessor.Design.Beam.RebarSelection.Validation
{
    /// <summary>
    /// Kiểm tra tính hợp lệ cơ bản của bố trí cốt thép dọc dầm.
    /// </summary>
    public sealed class RebarLayoutValidator
    {
        private readonly RebarSpacingChecker _spacingChecker;

        public RebarLayoutValidator()
        {
            _spacingChecker =
                new RebarSpacingChecker();
        }

        /// <summary>
        /// Kiểm tra toàn bộ bố trí cốt thép dọc của dầm.
        /// </summary>
        public bool Validate(
            BeamRebarLayoutModel layout,
            double beamWidth,
            double sideCover,
            double minSpacing)
        {
            if (layout == null)
            {
                throw new ArgumentNullException(
                    nameof(layout));
            }

            if (beamWidth <= 0.0)
            {
                throw new ArgumentException(
                    "Beam width must be > 0.",
                    nameof(beamWidth));
            }

            if (sideCover < 0.0)
            {
                throw new ArgumentException(
                    "Side cover must be >= 0.",
                    nameof(sideCover));
            }

            if (minSpacing < 0.0)
            {
                throw new ArgumentException(
                    "Minimum spacing must be >= 0.",
                    nameof(minSpacing));
            }

            if (!ValidateSide(
                layout.Top,
                beamWidth,
                sideCover,
                minSpacing))
            {
                return false;
            }

            if (!ValidateSide(
                layout.Bottom,
                beamWidth,
                sideCover,
                minSpacing))
            {
                return false;
            }

            return true;
        }

        private bool ValidateSide(
            RebarSide side,
            double beamWidth,
            double sideCover,
            double minSpacing)
        {
            if (side == null)
                return false;

            // =====================================================
            // CONTINUOUS
            // =====================================================

            if (!ValidateLayers(
                side.Continuous,
                beamWidth,
                sideCover,
                minSpacing))
            {
                return false;
            }

            // =====================================================
            // LEFT
            // =====================================================

            if (!ValidateLayers(
                side.Left,
                beamWidth,
                sideCover,
                minSpacing))
            {
                return false;
            }

            // =====================================================
            // MIDDLE
            // =====================================================

            if (!ValidateLayers(
                side.Middle,
                beamWidth,
                sideCover,
                minSpacing))
            {
                return false;
            }

            // =====================================================
            // RIGHT
            // =====================================================

            if (!ValidateLayers(
                side.Right,
                beamWidth,
                sideCover,
                minSpacing))
            {
                return false;
            }

            return true;
        }

        private bool ValidateLayers(
            System.Collections.Generic.IReadOnlyList<RebarLayer> layers,
            double beamWidth,
            double sideCover,
            double minSpacing)
        {
            if (layers == null)
                return false;

            foreach (RebarLayer layer in layers)
            {
                if (!ValidateLayer(
                    layer,
                    beamWidth,
                    sideCover,
                    minSpacing))
                {
                    return false;
                }
            }

            return true;
        }

        private bool ValidateLayer(
            RebarLayer layer,
            double beamWidth,
            double sideCover,
            double minSpacing)
        {
            if (layer == null)
                return false;

            if (layer.Reinforcements == null)
                return false;

            foreach (
                RebarReinforcement reinforcement
                in layer.Reinforcements)
            {
                if (!ValidateReinforcement(
                    reinforcement,
                    beamWidth,
                    sideCover,
                    minSpacing))
                {
                    return false;
                }
            }

            return true;
        }

        private bool ValidateReinforcement(
            RebarReinforcement reinforcement,
            double beamWidth,
            double sideCover,
            double minSpacing)
        {
            if (reinforcement == null)
                return false;

            if (reinforcement.BarCount <= 0)
                return false;

            if (reinforcement.Diameter <= 0.0)
                return false;

            if (reinforcement.Area <= 0.0)
                return false;

            return _spacingChecker.CanFit(
                beamWidth,
                sideCover,
                reinforcement.BarCount,
                reinforcement.Diameter,
                minSpacing);
        }
    }
}