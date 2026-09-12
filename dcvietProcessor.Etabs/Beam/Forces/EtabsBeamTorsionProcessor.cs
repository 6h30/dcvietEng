using System;
using dcvietProcessor.Etabs.Beam.Models;
using dcvietProcessor.Etabs.Common;

namespace dcvietProcessor.Etabs.Beam.Forces
{
    public sealed class EtabsBeamTorsionProcessor
    {
        private readonly EtabsAbsoluteEnvelope _start =
            new EtabsAbsoluteEnvelope();

        private readonly EtabsAbsoluteEnvelope _middle =
            new EtabsAbsoluteEnvelope();

        private readonly EtabsAbsoluteEnvelope _end =
            new EtabsAbsoluteEnvelope();

        public void Process(
            EtabsBeamRegion region,
            double torsion)
        {
            switch (region)
            {
                case EtabsBeamRegion.Start:
                    _start.Add(torsion);
                    break;

                case EtabsBeamRegion.Middle:
                    _middle.Add(torsion);
                    break;

                case EtabsBeamRegion.End:
                    _end.Add(torsion);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(region));
            }
        }

        public double Start
        {
            get { return _start.ValueOrZero(); }
        }

        public double Middle
        {
            get { return _middle.ValueOrZero(); }
        }

        public double End
        {
            get { return _end.ValueOrZero(); }
        }
    }
}