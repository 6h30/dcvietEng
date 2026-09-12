namespace dcvietProcessor.Design.Beam.DTO
{
    /// <summary>
    /// Các đại lượng diện tích và vị trí trọng tâm
    /// của cốt thép dọc dùng trong tính toán dầm.
    ///
    /// Quy ước:
    ///     TOP    = mép chịu nén
    ///     BOTTOM = mép chịu kéo
    ///
    ///     As      = diện tích cốt thép chịu kéo.
    ///     AsPrime = diện tích cốt thép chịu nén.
    /// </summary>
    public sealed class BeamEffectiveRebarPosition
    {
        /// <summary>
        /// Diện tích cốt thép chịu kéo, mm2.
        /// </summary>
        public double As { get; set; }

        /// <summary>
        /// Diện tích cốt thép chịu nén, mm2.
        /// </summary>
        public double AsPrime { get; set; }

        /// <summary>
        /// Khoảng cách từ mép chịu kéo
        /// đến trọng tâm cốt thép kéo, mm.
        ///
        /// Với dầm thông thường:
        ///     a = h - d
        /// </summary>
        public double a { get; set; }

        /// <summary>
        /// Khoảng cách từ mép chịu nén
        /// đến trọng tâm cốt thép nén, mm.
        ///
        /// Với dầm thông thường:
        ///     aPrime = dPrime
        /// </summary>
        public double aPrime { get; set; }

        /// <summary>
        /// Chiều cao làm việc của tiết diện, mm.
        ///
        /// Với dầm thông thường:
        ///     d = khoảng cách từ mép chịu nén
        ///         đến trọng tâm cốt thép kéo.
        /// </summary>
        public double d { get; set; }

        /// <summary>
        /// Khoảng cách từ mép chịu nén
        /// đến trọng tâm cốt thép nén, mm.
        /// </summary>
        public double dPrime { get; set; }

        /// <summary>
        /// Chiều cao làm việc hiệu dụng, mm.
        ///
        /// Với tiết diện chữ nhật thông thường:
        ///     h0 = d
        /// </summary>
        public double h0 { get; set; }
    }
}