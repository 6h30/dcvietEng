namespace dcvietProcessor.Design.Beam.Flexure
{
    /// <summary>
    /// Kết quả tính khả năng chịu uốn của tiết diện dầm.
    /// </summary>
    public sealed class BeamFlexureResult
    {
        /// <summary>
        /// Biến dạng giới hạn của cốt thép:
        ///
        /// epsilon_sel = Rs / Es
        /// </summary>
        public double EpsilonSel { get; set; }

        /// <summary>
        /// Biến dạng giới hạn của bê tông.
        ///
        /// Theo logic VBA hiện tại:
        /// epsilon_b2 = 0.0035
        /// </summary>
        public double EpsilonB2 { get; set; }

        /// <summary>
        /// Chiều cao tương đối giới hạn
        /// của vùng bê tông chịu nén.
        /// </summary>
        public double XiR { get; set; }

        /// <summary>
        /// Chiều cao tương đối thực tế
        /// của vùng bê tông chịu nén:
        ///
        /// Xi = X / H0
        /// </summary>
        public double Xi { get; set; }

        /// <summary>
        /// Chiều cao làm việc của tiết diện, mm.
        ///
        /// H0 = h - a
        /// </summary>
        public double H0 { get; set; }

        /// <summary>
        /// Chiều cao vùng bê tông chịu nén
        /// trước khi giới hạn bởi XiR, mm.
        /// </summary>
        public double XRaw { get; set; }

        /// <summary>
        /// Chiều cao vùng bê tông chịu nén
        /// được sử dụng trong tính toán, mm.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Chiều cao vùng nén giới hạn, mm.
        ///
        /// XLimit = XiR * H0
        /// </summary>
        public double XLimit { get; set; }

        /// <summary>
        /// Khả năng chịu moment của tiết diện, N.mm.
        /// </summary>
        public double MomentCapacity { get; set; }

        /// <summary>
        /// True khi xảy ra trường hợp:
        ///
        /// Rsc * As' >= Rs * As
        ///
        /// và công thức Mu chuyển sang:
        ///
        /// Mu = Rs * As * (H0 - a')
        /// </summary>
        public bool CompressionSteelControls { get; set; }
    }
}