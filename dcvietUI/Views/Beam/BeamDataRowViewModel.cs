
namespace dcvietUI.Views.Beam
{
    public sealed class BeamDataRowViewModel
    {
        // =====================================================
        // FORCE
        // =====================================================

        public string ForceType { get; set; }

        public double ForceLeft { get; set; }

        public double ForceMiddle { get; set; }

        public double ForceRight { get; set; }


        // =====================================================
        // REBAR
        // =====================================================

        public string MainRebar { get; set; }

        public string RebarLeft { get; set; }

        public string RebarMiddle { get; set; }

        public string RebarRight { get; set; }


        // =====================================================
        // FLEXURE
        // =====================================================

        public string FlexureType { get; set; }

        public double FlexureLeft { get; set; }

        public double FlexureMiddle { get; set; }

        public double FlexureRight { get; set; }


        // =====================================================
        // CRACK
        // =====================================================

        public string CrackType { get; set; }

        public double CrackLeft { get; set; }

        public double CrackMiddle { get; set; }

        public double CrackRight { get; set; }


        // =====================================================
        // CHECK
        // =====================================================

        public bool IsPassed { get; set; }


    }
}
