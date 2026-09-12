using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dcvietProcessor.Design.Beam.Crack
{
    /// <summary>
    /// Loại bề mặt cốt thép dọc
    /// sử dụng trong tính toán khe nứt.
    /// </summary>
    public enum RebarSurface
    {
        /// <summary>
        /// Cốt thép có gân.
        /// </summary>
        Ribbed,

        /// <summary>
        /// Cốt thép trơn.
        /// </summary>
        Plain
    }
}
