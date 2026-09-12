using System;
using System.Collections.Generic;
using System.Linq;

using dcvietProcessor.Etabs.Beam.Combination;
using dcvietProcessor.Etabs.Beam.Models;
using dcvietProcessor.Etabs.Beam.Processing;

namespace dcvietProcessor.Etabs.Beam.Pipeline
{
    public sealed class EtabsBeamProcessingPipeline
    {
        private readonly EtabsBeamGrouper _grouper;

        private readonly EtabsBeamStationProcessor _stationProcessor;

        private readonly EtabsBeamResultProcessor _resultProcessor;

        private readonly EtabsBeamResultAssembler _assembler;

        public EtabsBeamProcessingPipeline()
        {
            _grouper = new EtabsBeamGrouper();

            _stationProcessor =
                new EtabsBeamStationProcessor();

            _resultProcessor =
                new EtabsBeamResultProcessor(
                    _stationProcessor);

            _assembler =
                new EtabsBeamResultAssembler();
        }

        public IReadOnlyList<EtabsBeamProcessedResult> Process(
            EtabsBeamProcessingInput input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var mainBeams =
                _grouper.Group(input.Main);

            var f1Beams =
                _grouper.Group(input.F1);

            var f2Beams =
                _grouper.Group(input.F2);

            var f1Lookup =
                BuildLookup(f1Beams);

            var f2Lookup =
                BuildLookup(f2Beams);

            var results =
                new List<EtabsBeamProcessedResult>();

            foreach (var mainBeam in mainBeams)
            {
                var mainResult =
                    _resultProcessor.Process(mainBeam);

                if (mainResult == null)
                    continue;

                var key =
                    CreateKey(
                        mainBeam.Story,
                        mainBeam.Label);

                EtabsBeamProcessedResult f1Result = null;

                if (f1Lookup.TryGetValue(
                    key,
                    out var f1Beam))
                {
                    f1Result =
                        _resultProcessor.Process(f1Beam);
                }

                EtabsBeamProcessedResult f2Result = null;

                if (f2Lookup.TryGetValue(
                    key,
                    out var f2Beam))
                {
                    f2Result =
                        _resultProcessor.Process(f2Beam);
                }

                var finalResult =
                    _assembler.Assemble(
                        mainBeam,
                        mainResult,
                        f1Result,
                        f2Result);

                results.Add(finalResult);
            }

            return results;
        }

        private static Dictionary<string, EtabsBeamInput>
            BuildLookup(
                IEnumerable<EtabsBeamInput> beams)
        {
            return beams
                .GroupBy(x =>
                    CreateKey(x.Story, x.Label))
                .ToDictionary(
                    x => x.Key,
                    x => x.First());
        }

        private static string CreateKey(
            string story,
            string label)
        {
            return $"{Normalize(story)}|{Normalize(label)}";
        }

        private static string Normalize(
            string value)
        {
            return value?
                       .Trim()
                       .ToUpperInvariant()
                   ?? string.Empty;
        }
    }
}