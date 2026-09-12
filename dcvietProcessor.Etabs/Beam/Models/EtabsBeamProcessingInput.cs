using System.Collections.Generic;

namespace dcvietProcessor.Etabs.Beam.Models
{
    public sealed class EtabsBeamProcessingInput
    {
        public IReadOnlyList<EtabsBeamInput> Main { get; set; }
            = new List<EtabsBeamInput>();

        public IReadOnlyList<EtabsBeamInput> F1 { get; set; }
            = new List<EtabsBeamInput>();

        public IReadOnlyList<EtabsBeamInput> F2 { get; set; }
            = new List<EtabsBeamInput>();
    }
}