namespace dcvietProcessor.Etabs.Beam.Models
{
    public sealed class EtabsBeamProcessedResult
    {
        public string Label { get; set; }

        public string Story { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Length { get; set; }

        public EtabsBeamRegionResult Start { get; set; }

        public EtabsBeamRegionResult Middle { get; set; }

        public EtabsBeamRegionResult End { get; set; }

        public EtabsBeamAdditionalData AdditionalData { get; set; }
    }
}