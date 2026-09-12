using System.Collections.Generic;

namespace dcvietProcessor.Etabs.Beam.Models
{
    public sealed class EtabsBeamInput
    {
        public string Label { get; set; }

        public string Story { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public IReadOnlyList<EtabsBeamStationInput> Stations { get; set; }
            = new List<EtabsBeamStationInput>();

        public EtabsBeamAdditionalData AdditionalData { get; set; }
    }
}