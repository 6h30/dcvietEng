using System.Collections.Generic;

namespace dcvietProcessor.Etabs.Beam.Models
{
    public sealed class EtabsBeamStationInput
    {
        public double Station { get; set; }

        public IReadOnlyList<EtabsBeamForceInput> Forces { get; set; }
            = new List<EtabsBeamForceInput>();
    }
}