using System;
using dcvietProcessor.Etabs.Beam.Forces;
using dcvietProcessor.Etabs.Beam.Models;

namespace dcvietProcessor.Etabs.Beam.Combination
{
    public sealed class EtabsBeamForceSetProcessor
    {
        private readonly EtabsBeamMomentProcessor _moment;
        private readonly EtabsBeamShearProcessor _shear;
        private readonly EtabsBeamTorsionProcessor _torsion;

        public EtabsBeamForceSetProcessor()
        {
            _moment = new EtabsBeamMomentProcessor();
            _shear = new EtabsBeamShearProcessor();
            _torsion = new EtabsBeamTorsionProcessor();
        }

        public void Process(
            EtabsBeamRegion region,
            EtabsBeamForceInput force)
        {
            if (force == null)
                return;

            _moment.Process(region, force.M3);

            _shear.Process(region, force.V2);

            _torsion.Process(region, force.T);
        }

        public EtabsBeamRegionResult CreateResult(
            EtabsBeamRegion region)
        {
            switch (region)
            {
                case EtabsBeamRegion.Start:
                    return CreateStart();

                case EtabsBeamRegion.Middle:
                    return CreateMiddle();

                case EtabsBeamRegion.End:
                    return CreateEnd();

                default:
                    throw new ArgumentOutOfRangeException(nameof(region));
            }
        }

        private EtabsBeamRegionResult CreateStart()
        {
            return new EtabsBeamRegionResult
            {
                MMin = _moment.StartMin,
                MMax = _moment.StartMax,

                V = _shear.Start,

                T = _torsion.Start
            };
        }

        private EtabsBeamRegionResult CreateMiddle()
        {
            return new EtabsBeamRegionResult
            {
                MMin = _moment.MiddleMin,
                MMax = _moment.MiddleMax,

                V = _shear.Middle,

                T = _torsion.Middle
            };
        }

        private EtabsBeamRegionResult CreateEnd()
        {
            return new EtabsBeamRegionResult
            {
                MMin = _moment.EndMin,
                MMax = _moment.EndMax,

                V = _shear.End,

                T = _torsion.End
            };
        }
    }
}