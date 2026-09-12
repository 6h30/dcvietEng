using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dcvietProcessor.Design.DTO
{
    public sealed class ConcreteMaterialInput
    {
        public string Grade { get; set; }

        public double Rb { get; set; }

        public double RbSer { get; set; }

        public double Rbt { get; set; }

        public double RbtSer { get; set; }

        public double Eb { get; set; }
    }
}
