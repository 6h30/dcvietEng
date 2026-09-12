using System;
using dcvietProcessor.Etabs.Beam.Models;
using dcvietProcessor.Etabs.Common;

namespace dcvietProcessor.Etabs.Beam.Forces
{
    public sealed class EtabsBeamMomentProcessor
    {
        private readonly EtabsForceEnvelope _start =
            new EtabsForceEnvelope();

        private readonly EtabsForceEnvelope _middle =
            new EtabsForceEnvelope();

        private readonly EtabsForceEnvelope _end =
            new EtabsForceEnvelope();

        public void Process(
            EtabsBeamRegion region,
            double moment)
        {
            switch (region)
            {
                case EtabsBeamRegion.Start:
                    _start.Add(moment);
                    break;

                case EtabsBeamRegion.Middle:
                    _middle.Add(moment);
                    break;

                case EtabsBeamRegion.End:
                    _end.Add(moment);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(region));
            }
        }

        public EtabsBeamRegionResult Apply(
            EtabsBeamRegionResult source)
        {
            return new EtabsBeamRegionResult
            {
                MMin = source.MMin,
                MMax = source.MMax,

                F1Min = source.F1Min,
                F1Max = source.F1Max,

                F2Min = source.F2Min,
                F2Max = source.F2Max,

                V = source.V,
                T = source.T
            };
        }

        public double StartMin
        {
            get { return _start.MinOrZero(); }
        }

        public double StartMax
        {
            get { return _start.MaxOrZero(); }
        }

        public double MiddleMin
        {
            get { return _middle.MinOrZero(); }
        }

        public double MiddleMax
        {
            get { return _middle.MaxOrZero(); }
        }

        public double EndMin
        {
            get { return _end.MinOrZero(); }
        }

        public double EndMax
        {
            get { return _end.MaxOrZero(); }
        }
    }
}