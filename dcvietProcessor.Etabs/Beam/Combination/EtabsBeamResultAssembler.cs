using System;
using dcvietProcessor.Etabs.Beam.Models;

namespace dcvietProcessor.Etabs.Beam.Combination
{
    public sealed class EtabsBeamResultAssembler
    {
        public EtabsBeamProcessedResult Assemble(
            EtabsBeamInput main,
            EtabsBeamProcessedResult mainResult,
            EtabsBeamProcessedResult f1Result,
            EtabsBeamProcessedResult f2Result)
        {
            if (main == null)
                throw new ArgumentNullException(nameof(main));

            if (mainResult == null)
                throw new ArgumentNullException(nameof(mainResult));

            return new EtabsBeamProcessedResult
            {
                Label = mainResult.Label,
                Story = mainResult.Story,

                Width = mainResult.Width,
                Height = mainResult.Height,

                Length = mainResult.Length,

                Start = MergeRegion(
                    mainResult.Start,
                    f1Result?.Start,
                    f2Result?.Start),

                Middle = MergeRegion(
                    mainResult.Middle,
                    f1Result?.Middle,
                    f2Result?.Middle),

                End = MergeRegion(
                    mainResult.End,
                    f1Result?.End,
                    f2Result?.End),

                AdditionalData = main.AdditionalData
            };
        }

        private static EtabsBeamRegionResult MergeRegion(
            EtabsBeamRegionResult main,
            EtabsBeamRegionResult f1,
            EtabsBeamRegionResult f2)
        {
            return new EtabsBeamRegionResult
            {
                MMin = main?.MMin ?? 0.0,
                MMax = main?.MMax ?? 0.0,

                F1Min = f1?.MMin ?? 0.0,
                F1Max = f1?.MMax ?? 0.0,

                F2Min = f2?.MMin ?? 0.0,
                F2Max = f2?.MMax ?? 0.0,

                V = main?.V ?? 0.0,
                T = main?.T ?? 0.0
            };
        }
    }
}