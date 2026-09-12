using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dcvietProcessor.Design.DTO
{
    public sealed class SteelMaterialInput
    {
        /// <summary>
        /// Cấp độ bền cốt thép.
        /// Ví dụ: CB500-V, CB300-V
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// Cường độ chịu kéo tính toán, MPa.
        /// Rs
        /// </summary>
        public double Rs { get; set; }

        /// <summary>
        /// Cường độ chịu nén tính toán, MPa.
        /// Rsc
        /// </summary>
        public double Rsc { get; set; }

        /// <summary>
        /// Cường độ tính toán của cốt thép ngang, MPa.
        ///
        /// Dùng cho cốt đai trong tính toán chịu cắt.
        /// Rsw
        /// </summary>
        public double Rsw { get; set; }

        /// <summary>
        /// Mô đun đàn hồi của cốt thép, MPa.
        /// Es
        /// </summary>
        public double Es { get; set; }
    }
}
