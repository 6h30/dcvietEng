namespace dcvietProcessor.Design.Beam.RebarSelection.Models
{
    public sealed class RebarSelectionResult
    {
        public bool IsSuccess { get; }

        public RebarCandidate Candidate { get; }

        public double RequiredArea { get; }

        public double ProvidedArea
        {
            get
            {
                if (Candidate == null)
                    return 0.0;

                return Candidate.Area;
            }
        }

        public double ExcessArea
        {
            get
            {
                return ProvidedArea - RequiredArea;
            }
        }

        public RebarSelectionResult(
            bool isSuccess,
            RebarCandidate candidate,
            double requiredArea)
        {
            IsSuccess = isSuccess;
            Candidate = candidate;
            RequiredArea = requiredArea;
        }
    }
}