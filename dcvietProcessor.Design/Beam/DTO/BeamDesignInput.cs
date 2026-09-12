using BeamRebarLayoutModel =
    dcvietProcessor.Design.Beam.RebarSelection.BeamRebarLayout.BeamRebarLayout;

namespace dcvietProcessor.Design.Beam.DTO
{
    public sealed class BeamDesignInput
    {
        /// <summary>
        /// Chiều rộng tiết diện, mm.
        /// Được lấy từ dữ liệu ETABS đã xử lý.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Chiều cao tiết diện, mm.
        /// Được lấy từ dữ liệu ETABS đã xử lý.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Chiều dài dầm, mm.
        /// </summary>
        public double Length { get; set; }

        // Steel geometry
        public double TensileSteelCentroid { get; set; }
        public double CompressionSteelCentroid { get; set; }

        /// <summary>
        /// Phương án bố trí cốt thép đã được lựa chọn.
        /// </summary>
        public BeamRebarLayoutModel RebarLayout { get; set; }

        // Forces
        public BeamDesignForceSet Forces { get; set; }

        // Material đã resolve
        public BeamResolvedMaterialInput Material { get; set; }
    }
}