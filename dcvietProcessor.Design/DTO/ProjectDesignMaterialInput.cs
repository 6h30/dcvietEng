using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dcvietProcessor.Design.DTO
{
    public sealed class ProjectDesignMaterialInput
    {
        /// <summary>
        /// Cấp độ bền bê tông sử dụng cho cấu kiện.
        /// Ví dụ: B30, B35, B40...
        /// </summary>
        public string ConcreteGrade { get; set; }

        /// <summary>
        /// Cấp độ bền cốt thép dọc.
        /// Ví dụ: CB400-V, CB500-V...
        /// </summary>
        public string SteelMainGrade { get; set; }

        /// <summary>
        /// Cấp độ bền cốt thép đai.
        /// Ví dụ: CB240-T, CB300-V...
        /// </summary>
        public string SteelStirrupGrade { get; set; }
    }
}
