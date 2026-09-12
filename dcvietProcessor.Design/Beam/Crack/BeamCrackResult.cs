using System;

namespace dcvietProcessor.Design.Beam.Crack
{
    public sealed class BeamCrackResult
    {
        /// <summary>
        /// Bề rộng khe nứt ngắn hạn.
        /// a_crc1 + a_crc2 - a_crc3
        /// </summary>
        public double ShortTerm { get; set; }

        /// <summary>
        /// Bề rộng khe nứt dài hạn.
        /// a_crc1
        /// </summary>
        public double LongTerm { get; set; }

        /// <summary>
        /// a_crc1
        /// </summary>
        public double Crack1 { get; set; }

        /// <summary>
        /// a_crc2
        /// </summary>
        public double Crack2 { get; set; }

        /// <summary>
        /// a_crc3
        /// </summary>
        public double Crack3 { get; set; }
    }
}