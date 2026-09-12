using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dcvietProcessor.Design.DTO
{
    public sealed class TCVNParametersInput
    {
        /// <summary>
        /// Giới hạn bề rộng khe nứt cho tác dụng ngắn hạn, mm.
        /// Theo Bảng 17 - TCVN 5574:2018.
        /// </summary>
        public double ShortTermCrackLimit { get; set; }

        /// <summary>
        /// Giới hạn bề rộng khe nứt cho tác dụng dài hạn, mm.
        /// Theo Bảng 17 - TCVN 5574:2018.
        /// </summary>
        public double LongTermCrackLimit { get; set; }
    }
}
