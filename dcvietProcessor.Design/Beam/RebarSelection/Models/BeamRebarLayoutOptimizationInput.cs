namespace dcvietProcessor.Design.Beam.RebarSelection.Models
{
    public sealed class BeamRebarLayoutOptimizationInput
    {
        public RebarSideRequiredArea Top { get; set; }

        public RebarSideRequiredArea Bottom { get; set; }

        /// <summary>
        /// Điều kiện chung dùng khi chọn thép chạy suốt.
        /// RequiredArea sẽ do Optimizer gán lại.
        /// </summary>
        public RebarSelectionInput ContinuousSelection { get; set; }

        /// <summary>
        /// Điều kiện chung dùng khi chọn thép gia cường.
        /// RequiredArea sẽ do Optimizer gán lại.
        /// </summary>
        public RebarSelectionInput AdditionalSelection { get; set; }
    }
}