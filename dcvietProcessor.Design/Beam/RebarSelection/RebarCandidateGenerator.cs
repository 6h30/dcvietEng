using System;
using System.Collections.Generic;
using dcvietProcessor.Design.Beam.RebarSelection.Models;

namespace dcvietProcessor.Design.Beam.RebarSelection
{
    public sealed class RebarCandidateGenerator
    {
        private static readonly double[] DefaultDiameters =
        {
            12.0,
            14.0,
            16.0,
            18.0,
            20.0,
            22.0,
            25.0,
            28.0,
            32.0
        };

        public IList<RebarCandidate> Generate(
            RebarSelectionInput input)
        {
            ValidateInput(input);

            double[] diameters =
                GetDiameters(input);

            var candidates =
                new List<RebarCandidate>();

            for (
                int barCount = input.MinBarCount;
                barCount <= input.MaxBarCount;
                barCount++)
            {
                foreach (double diameter in diameters)
                {
                    if (diameter < input.MinDiameter)
                        continue;

                    if (diameter > input.MaxDiameter)
                        continue;

                    candidates.Add(
                        new RebarCandidate(
                            barCount,
                            diameter));
                }
            }

            return candidates;
        }

        private double[] GetDiameters(
            RebarSelectionInput input)
        {
            if (input.AllowedDiameters != null &&
                input.AllowedDiameters.Length > 0)
            {
                return input.AllowedDiameters;
            }

            return DefaultDiameters;
        }

        private void ValidateInput(
            RebarSelectionInput input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (input.RequiredArea <= 0.0)
                throw new ArgumentException(
                    "RequiredArea must be > 0.");

            if (input.MinDiameter <= 0.0)
                throw new ArgumentException(
                    "MinDiameter must be > 0.");

            if (input.MaxDiameter < input.MinDiameter)
                throw new ArgumentException(
                    "MaxDiameter must be >= MinDiameter.");

            if (input.MinBarCount <= 0)
                throw new ArgumentException(
                    "MinBarCount must be > 0.");

            if (input.MaxBarCount < input.MinBarCount)
                throw new ArgumentException(
                    "MaxBarCount must be >= MinBarCount.");
        }
    }
}