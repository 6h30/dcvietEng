using System.Collections.ObjectModel;
using System.Linq;

namespace dcvietUI.Views.Beam
{
    public sealed class BeamTableItemViewModel
    {
        public string Story { get; set; }

        public string BeamEtabsName { get; set; }

        public string BeamDrawingName { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }


        // =====================================================
        // ROW DATA
        // =====================================================

        public ObservableCollection<BeamDataRowViewModel> Rows { get; set; }
            = new ObservableCollection<BeamDataRowViewModel>();


        // =====================================================
        // GROUP CHECK
        // =====================================================

        /// <summary>
        /// Kết quả gộp của Row 0 và Row 1.
        /// Chỉ đạt khi cả hai row đều đạt.
        /// </summary>
        public bool IsGroup1Passed => AreRowsPassed(0, 1);

        /// <summary>
        /// Kết quả của Row 2.
        /// </summary>
        public bool IsGroup2Passed => AreRowsPassed(2);

        /// <summary>
        /// Kết quả gộp của Row 3 và Row 4.
        /// Chỉ đạt khi cả hai row đều đạt.
        /// </summary>
        public bool IsGroup3Passed => AreRowsPassed(3, 4);

        /// <summary>
        /// Kết quả của Row 5.
        /// </summary>
        public bool IsGroup4Passed => AreRowsPassed(5);

        /// <summary>
        /// Kết quả gộp của Row 6 và Row 7.
        /// Chỉ đạt khi cả hai row đều đạt.
        /// </summary>
        public bool IsGroup5Passed => AreRowsPassed(6, 7);


        // =====================================================
        // CHECK TEXT
        // =====================================================

        public string Group1CheckText => GetCheckText(IsGroup1Passed);

        public string Group2CheckText => GetCheckText(IsGroup2Passed);

        public string Group3CheckText => GetCheckText(IsGroup3Passed);

        public string Group4CheckText => GetCheckText(IsGroup4Passed);

        public string Group5CheckText => GetCheckText(IsGroup5Passed);


        // =====================================================
        // PRIVATE METHODS
        // =====================================================

        private bool AreRowsPassed(params int[] indexes)
        {
            if (indexes == null || indexes.Length == 0)
                return false;

            return indexes.All(index =>
                index >= 0 &&
                index < Rows.Count &&
                Rows[index].IsPassed);
        }

        private static string GetCheckText(bool isPassed)
        {
            return isPassed ? "OK" : "NOT";
        }
    }
}