
using System.Collections.ObjectModel;
using System.Windows.Controls;
using dcvietUI.Views.Beam;

namespace dcvietUI.Views.Beam
{
    public partial class BeamDataBlockView : UserControl
    {
        public BeamDataBlockView()
        {
            InitializeComponent();

            // TEMP:
            // Chỉ dùng để kiểm tra giao diện.
            DataContext = CreateSampleBeam();
        }


        private BeamTableItemViewModel CreateSampleBeam()
        {
            return new BeamTableItemViewModel
            {
                Story = "L10",

                BeamEtabsName = "B125",

                BeamDrawingName = "P2.L10.HB3.2",

                Width = 600,

                Height = 800,


                Rows = new ObservableCollection<BeamDataRowViewModel>
{
    // =================================================
    // ROW 1: THÉP TRÊN - LỚP 1
    // =================================================

    new BeamDataRowViewModel
    {
        ForceType = "Mtop (kN.m)",
        ForceLeft = -420.5,
        ForceMiddle = -82.2,
        ForceRight = -350.8,

        MainRebar = "2D20",
        RebarLeft = "3D25",
        RebarMiddle = "-",
        RebarRight = "3D25",

        FlexureType = "a (mm)",
        FlexureLeft = 55.0,
        FlexureMiddle = 50.0,
        FlexureRight = 55.0,

        CrackType = "[M] (kN.m)",
        CrackLeft = 515.4,
        CrackMiddle = 245.8,
        CrackRight = 488.6,

        IsPassed = true
    },


    // =================================================
    // ROW 2: THÉP TRÊN - LỚP 2
    // =================================================

    new BeamDataRowViewModel
    {
        ForceType = "Mbot (kN.m)",
        ForceLeft = 95.4,
        ForceMiddle = 285.3,
        ForceRight = 110.1,

        MainRebar = "",
        RebarLeft = "2D20",
        RebarMiddle = "-",
        RebarRight = "2D20",

        FlexureType = "As (mm2)",
        FlexureLeft = 2419.0,
        FlexureMiddle = 628.0,
        FlexureRight = 2419.0,

        CrackType = "M/[M]",
        CrackLeft = 0.816,
        CrackMiddle = 0.334,
        CrackRight = 0.718,

        IsPassed = true
    },


    // =================================================
    // ROW 3: THÉP TRÊN - LỚP 3
    // =================================================

    new BeamDataRowViewModel
    {
        ForceType = "M1.Top (kN.m)",
        ForceLeft = -315.6,
        ForceMiddle = -65.2,
        ForceRight = -298.7,

        MainRebar = "",
        RebarLeft = "-",
        RebarMiddle = "-",
        RebarRight = "-",

        FlexureType = "μ (%)",
        FlexureLeft = 1.48,
        FlexureMiddle = 0.38,
        FlexureRight = 1.48,

        CrackType = "acrc (mm)",
        CrackLeft = 0.182,
        CrackMiddle = 0.115,
        CrackRight = 0.171,

        IsPassed = true
    },


    // =================================================
    // ROW 4: THÉP DƯỚI - LỚP 1
    // =================================================

    new BeamDataRowViewModel
    {
        ForceType = "M1.Bot (kN.m)",
        ForceLeft = 85.5,
        ForceMiddle = 324.2,
        ForceRight = 92.8,

        MainRebar = "3D25",
        RebarLeft = "-",
        RebarMiddle = "2D25",
        RebarRight = "-",

        FlexureType = "a (mm)",
        FlexureLeft = 50.0,
        FlexureMiddle = 55.0,
        FlexureRight = 50.0,

        CrackType = "[M] (kN.m)",
        CrackLeft = 235.6,
        CrackMiddle = 438.5,
        CrackRight = 235.6,

        IsPassed = true
    },


    // =================================================
    // ROW 5: THÉP DƯỚI - LỚP 2
    // =================================================

    new BeamDataRowViewModel
    {
        ForceType = "M2.Top (kN.m)",
        ForceLeft = -240.1,
        ForceMiddle = -52.4,
        ForceRight = -210.9,

        MainRebar = "2D20",
        RebarLeft = "-",
        RebarMiddle = "-",
        RebarRight = "-",

        FlexureType = "As (mm2)",
        FlexureLeft = 1256.0,
        FlexureMiddle = 1923.0,
        FlexureRight = 1256.0,

        CrackType = "M/[M]",
        CrackLeft = 0.363,
        CrackMiddle = 0.739,
        CrackRight = 0.394,

        IsPassed = true
    },


    // =================================================
    // ROW 6: THÉP DƯỚI - LỚP 3
    // =================================================

    new BeamDataRowViewModel
    {
        ForceType = "M2.Bot (kN.m)",
        ForceLeft = 72.5,
        ForceMiddle = 248.4,
        ForceRight = 80.8,

        MainRebar = "",
        RebarLeft = "-",
        RebarMiddle = "-",
        RebarRight = "-",

        FlexureType = "μ (%)",
        FlexureLeft = 0.77,
        FlexureMiddle = 1.18,
        FlexureRight = 0.77,

        CrackType = "acrc (mm)",
        CrackLeft = 0.148,
        CrackMiddle = 0.196,
        CrackRight = 0.145,

        IsPassed = true
    },


    // =================================================
    // ROW 7: THÉP ĐAI
    // =================================================

    new BeamDataRowViewModel
    {
        ForceType = "V (kN)",
        ForceLeft = 318.2,
        ForceMiddle = 108.5,
        ForceRight = 297.6,

        MainRebar = "",
        RebarLeft = "D10a100",
        RebarMiddle = "D10a150",
        RebarRight = "D10a100",

        FlexureType = "[V] (kN)",
        FlexureLeft = 425.4,
        FlexureMiddle = 330.2,
        FlexureRight = 425.4,

        CrackType = "V/[V]",
        CrackLeft = 0.748,
        CrackMiddle = 0.329,
        CrackRight = 0.700,

        IsPassed = true
    },


    // =================================================
    // ROW 8: XOẮN / KIỂM TRA TƯƠNG TÁC
    // =================================================

    new BeamDataRowViewModel
    {
        ForceType = "T (kN.m)",
        ForceLeft = 42.5,
        ForceMiddle = 24.2,
        ForceRight = 35.8,

        MainRebar = "",
        RebarLeft = "-",
        RebarMiddle = "-",
        RebarRight = "-",

        FlexureType = "smax (mm)",
        FlexureLeft = 100.0,
        FlexureMiddle = 150.0,
        FlexureRight = 100.0,

        CrackType = "Ratio V-T",
        CrackLeft = 1.082,
        CrackMiddle = 0.704,
        CrackRight = 0.888,

        // Left = 1.082 > 1.0 nên cố ý không đạt
        IsPassed = false
    }
}
            };
        }
    }
}

