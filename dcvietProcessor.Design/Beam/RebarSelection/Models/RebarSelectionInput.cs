namespace dcvietProcessor.Design.Beam.RebarSelection.Models
{
    /// <summary>
    /// Điều kiện đầu vào cho một lần chọn phương án cốt thép.
    ///
    /// Class này không biết:
    /// - TOP / BOTTOM;
    /// - Left / Middle / Right;
    /// - Continuous / Additional;
    /// - moment âm / dương.
    ///
    /// Nó chỉ mô tả:
    ///     As yêu cầu
    ///     + giới hạn số thanh
    ///     + giới hạn đường kính.
    /// </summary>
    public sealed class RebarSelectionInput
    {
        /// <summary>
        /// Diện tích cốt thép yêu cầu
        /// cho lần selection hiện tại, mm2.
        /// </summary>
        public double RequiredArea { get; set; }

        /// <summary>
        /// Đường kính nhỏ nhất được phép, mm.
        /// </summary>
        public double MinDiameter { get; set; }

        /// <summary>
        /// Đường kính lớn nhất được phép, mm.
        /// </summary>
        public double MaxDiameter { get; set; }

        /// <summary>
        /// Số thanh nhỏ nhất được phép.
        /// </summary>
        public int MinBarCount { get; set; }

        /// <summary>
        /// Số thanh lớn nhất được phép.
        /// </summary>
        public int MaxBarCount { get; set; }

        /// <summary>
        /// Danh sách đường kính được phép sử dụng.
        ///
        /// Nếu null hoặc rỗng,
        /// CandidateGenerator có thể dùng
        /// tập đường kính mặc định.
        /// </summary>
        public double[] AllowedDiameters { get; set; }
    }
}