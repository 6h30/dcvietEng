using dcvietProcessor.Design.DTO;

namespace dcvietProcessor.Design.Beam.DTO
{
    /// <summary>
    /// Vật liệu đã được resolve cho một cấu kiện dầm.
    ///
    /// Class này không thực hiện tra cứu vật liệu.
    /// Dữ liệu được cung cấp từ Material Resolver.
    /// </summary>
    public sealed class BeamResolvedMaterialInput
    {
        /// <summary>
        /// Bê tông sử dụng cho dầm.
        /// </summary>
        public ConcreteMaterialInput Concrete { get; set; }

        /// <summary>
        /// Cốt thép dọc.
        /// </summary>
        public SteelMaterialInput MainSteel { get; set; }

        /// <summary>
        /// Cốt thép đai.
        /// </summary>
        public SteelMaterialInput StirrupSteel { get; set; }
    }
}