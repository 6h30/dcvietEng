using System;
using dcvietProcessor.Etabs.Beam.Models;
using dcvietProcessor.Etabs.Common;

namespace dcvietProcessor.Etabs.Beam.Forces
{
    public sealed class EtabsBeamShearProcessor
    {
        private readonly EtabsAbsoluteEnvelope _start =
            new EtabsAbsoluteEnvelope();

        private readonly EtabsAbsoluteEnvelope _middle =
            new EtabsAbsoluteEnvelope();

        private readonly EtabsAbsoluteEnvelope _end =
            new EtabsAbsoluteEnvelope();

        public void Process(
            EtabsBeamRegion region,
            double shear)
        {
            switch (region)
            {
                case EtabsBeamRegion.Start:
                    _start.Add(shear);
                    break;

                case EtabsBeamRegion.Middle:
                    _middle.Add(shear);
                    break;

                case EtabsBeamRegion.End:
                    _end.Add(shear);
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