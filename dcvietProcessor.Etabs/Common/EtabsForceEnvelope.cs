namespace dcvietProcessor.Etabs.Common
{
    internal sealed class EtabsForceEnvelope
    {
        public bool HasValue { get; private set; }

        public double Min { get; private set; }

        public double Max { get; private set; }

        public void Add(double value)
        {
            if (!HasValue)
            {
                Min = value;
                Max = value;
                HasValue = true;
                return;
            }

            if (value < Min)
                Min = value;

            if (value > Max)
                Max = value;
        }

        public double MinOrZero()
        {
            return HasValue ? Min : 0.0;
        }

        public double MaxOrZero()
        {
            return HasValue ? Max : 0.0;
        }
    }
}