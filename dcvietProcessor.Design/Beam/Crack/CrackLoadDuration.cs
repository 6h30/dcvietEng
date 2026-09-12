using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dcvietProcessor.Design.Beam.Crack
{
    /// <summary>
    /// Thời gian tác dụng của tải trọng
    /// dùng trong tính toán bề rộng khe nứt.
    /// </summary>
    public enum CrackLoadDuration
    {
        /// <summary>
        /// Tính toán ngắn hạn.
        /// </summary>
        ShortTerm = 1,

        /// <summary>
        /// Tính toán dài hạn.
        /// </summary>
        LongTerm = 2
    }
}
