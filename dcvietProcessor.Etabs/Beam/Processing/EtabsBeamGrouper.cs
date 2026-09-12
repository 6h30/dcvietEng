using System;
using System.Collections.Generic;
using System.Linq;
using dcvietProcessor.Etabs.Beam.Models;

namespace dcvietProcessor.Etabs.Beam.Processing
{
    public sealed class EtabsBeamGrouper
    {
        public IReadOnlyList<EtabsBeamInput> Group(
            IEnumerable<EtabsBeamInput> beams)
        {
            if (beams == null)
                throw new ArgumentNullException(nameof(beams));

            return beams
                .Where(x => x != null)
                .GroupBy(x => new BeamKey(
                    Normalize(x.Story),
                    Normalize(x.Label)))
                .Select(CreateBeam)
                .ToList();
        }

        private static EtabsBeamInput CreateBeam(
            IGrouping<BeamKey, EtabsBeamInput> group)
        {
            var first = group.First();

            var stations = group
                .SelectMany(x =>
                    x.Stations ?? Array.Empty<EtabsBeamStationInput>())
                .OrderBy(x => x.Station)
                .ToList();

            return new EtabsBeamInput
            {
                Label = first.Label?.Trim(),
                Story = first.Story?.Trim(),

                Width = first.Width,
                Height = first.Height,

                Stations = stations,

                AdditionalData = first.AdditionalData
            };
        }

        private static string Normalize(string value)
        {
            return value?.Trim() ?? string.Empty;
        }

        private sealed class BeamKey : IEquatable<BeamKey>
        {
            public string Story { get; }

            public string Label { get; }

            public BeamKey(
                string story,
                string label)
            {
                Story = story;
                Label = label;
            }

            public bool Equals(BeamKey other)
            {
                if (other == null)
                    return false;

                return string.Equals(
                           Story,
                           other.Story,
                           StringComparison.OrdinalIgnoreCase)
                       &&
                       string.Equals(
                           Label,
                           other.Label,
                           StringComparison.OrdinalIgnoreCase);
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as BeamKey);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;

                    hash = hash * 31 +
                           StringComparer.OrdinalIgnoreCase
                               .GetHashCode(Story);

                    hash = hash * 31 +
                           StringComparer.OrdinalIgnoreCase
                               .GetHashCode(Label);

                    return hash;
                }
            }
        }
    }
}