namespace dcvietProcessor.Etabs.Beam.Models
{
    public sealed class EtabsBeamForceInput
    {
        public string LoadCase { get; set; }

        public string StepType { get; set; }

        public int StepNumber { get; set; }

        public double P { get; set; }

        public double V2 { get; set; }

        public double V3 { get; set; }

        public double T { get; set; }

        public double M2 { get; set; }
            
        public double M3 { get; set; }
    }
}