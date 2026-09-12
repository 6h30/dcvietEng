namespace dcvietProcessor.Design.Beam.DTO
{
    /// <summary>
    /// Nội lực thiết kế tại một vị trí/miền của dầm.
    ///
    /// Đây là DTO thuộc Design layer.
    /// Không phụ thuộc ETABS.
    ///
    /// Đối với kiểm tra nứt, giữ nguyên hệ nội lực
    /// M1 / M2 / M3 / Mh1 / Mh2 theo logic VBA.
    /// </summary>
    public sealed class BeamDesignForceInput
    {
        // =========================================================
        // CRACK MOMENT
        // =========================================================

        /// <summary>
        /// Mô men M1 sử dụng cho kiểm tra nứt.
        ///
        /// VBA:
        /// a_crc2 = acrci(Abs(M1), ..., 1, rbt, Abs(Mh1))
        /// </summary>
        public double M1 { get; set; }

        /// <summary>
        /// Mô men M2 sử dụng cho kiểm tra nứt.
        ///
        /// VBA:
        /// a_crc3 = acrci(Abs(M2), ..., 1, rbt, Abs(Mh1))
        /// </summary>
        public double M2 { get; set; }

        /// <summary>
        /// Mô men M3 sử dụng cho kiểm tra nứt.
        ///
        /// VBA:
        /// a_crc1 = acrci(Abs(M3), ..., 2, rbt, Abs(Mh2))
        /// </summary>
        public double M3 { get; set; }

        // =========================================================
        // CRACK MOMENT - AUXILIARY
        // =========================================================

        /// <summary>
        /// Mô men Mh1 sử dụng cho Crack2 và Crack3.
        ///
        /// VBA:
        /// a_crc2 = acrci(Abs(M1), ..., 1, rbt, Abs(Mh1))
        /// a_crc3 = acrci(Abs(M2), ..., 1, rbt, Abs(Mh1))
        /// </summary>
        public double Mh1 { get; set; }

        /// <summary>
        /// Mô men Mh2 sử dụng cho Crack1.
        ///
        /// VBA:
        /// a_crc1 = acrci(Abs(M3), ..., 2, rbt, Abs(Mh2))
        /// </summary>
        public double Mh2 { get; set; }

        // =========================================================
        // SHEAR
        // =========================================================

        /// <summary>
        /// Lực cắt.
        /// </summary>
        public double V { get; set; }

        // =========================================================
        // TORSION
        // =========================================================

        /// <summary>
        /// Mô men xoắn.
        /// </summary>
        public double T { get; set; }
    }
}