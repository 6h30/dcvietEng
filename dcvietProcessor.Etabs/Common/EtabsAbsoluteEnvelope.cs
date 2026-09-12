using System;

namespace dcvietProcessor.Etabs.Common
{
    internal sealed class EtabsAbsoluteEnvelope
    {
        public bool HasValue { get; private set; }

        public double Value { get; private set; }

        public void Add(double value)
        {
            if (!HasValue)
            {
                Value = value;
                HasValue = true;
                return;
            }

            if (Math.Abs(value) > Math.Abs(Value))
            {
                Value = value;
            }
        }

        public double ValueOrZero()
        {
            return HasValue ? Value : 0.0;
        }
    }
}