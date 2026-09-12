namespace dcvietProcessor.Design.Beam.RebarSelection.Models
{
    /// <summary>
    /// Diện tích cốt thép yêu cầu tại ba vùng
    /// của một phía dầm.
    /// </summary>
    public sealed class RebarSideRequiredArea
    {
        /// <summary>
        /// As yêu cầu tại đầu trái, mm2.
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// As yêu cầu tại giữa nhịp, mm2.
        /// </summary>
        public double Middle { get; set; }

        /// <summary>
        /// As yêu cầu tại đầu phải, mm2.
        /// </summary>
        public double Right { get; set; }
    }
}