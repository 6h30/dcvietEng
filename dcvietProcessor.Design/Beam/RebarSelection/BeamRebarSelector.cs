using System;
using System.Linq;
using dcvietProcessor.Design.Beam.RebarSelection.Models;

namespace dcvietProcessor.Design.Beam.RebarSelection
{
    public sealed class BeamRebarSelector
    {
        private readonly RebarCandidateGenerator _generator;

        public BeamRebarSelector()
        {
            _generator =
                new RebarCandidateGenerator();
        }

        public RebarSelectionResult Select(
            RebarSelectionInput input)
        {
            if (input == null)
                throw new ArgumentNullException(
                    nameof(input));

            var candidates =
                _generator.Generate(input);

            var candidate =
                candidates
                    .Where(x =>
                        x.Area >= input.RequiredArea)

                    // 1. Ưu tiên diện tích dư nhỏ
                    .OrderBy(x =>
                        x.Area - input.RequiredArea)

                    // 2. Nếu gần tương đương,
                    // ưu tiên ít thanh hơn
                    .ThenBy(x =>
                        x.BarCount)

                    // 3. Sau cùng mới xét đường kính
                    .ThenBy(x =>
                        x.Diameter)

                    .FirstOrDefault();

            if (candidate == null)
            {
                return new RebarSelectionResult(
                    false,
                    null,
                    input.RequiredArea);
            }

            return new RebarSelectionResult(
                true,
                candidate,
                input.RequiredArea);
        }
    }
}