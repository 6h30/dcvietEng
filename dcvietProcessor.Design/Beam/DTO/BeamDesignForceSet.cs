namespace dcvietProcessor.Design.Beam.DTO
{
    /// <summary>
    /// Nội lực thiết kế của dầm tại các vùng Start / Middle / End.
    /// </summary>
    public sealed class BeamDesignForceSet
    {
        public BeamDesignForceInput Start { get; set; }

        public BeamDesignForceInput Middle { get; set; }

        public BeamDesignForceInput End { get; set; }
    }
}