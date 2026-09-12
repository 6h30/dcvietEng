using dcvietProcessor.Design.Beam.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dcvietProcessor.Design.Beam.Crack.Calculator
{
    public sealed class BeamCrackNeutralAxisCalculator
    {
        private const double Tolerance = 1e-9;

        public double Calculate(
            BeamDesignInput input,
            BeamEffectiveRebarPosition rebar,
            double alphaS1,
            double alphaS2)
        {
            double b = input.Width;
            double h = input.Height;
            /// double h0 = rebar.h0;
            double h0 = h - rebar.a;

            if (h0 <= Tolerance)
                return 0.0;

            double muS =
                rebar.As / (b * h0);

            double compressionHeight =
                h - rebar.aPrime;

            if (compressionHeight <= Tolerance)
                return 0.0;

            double muSPrime =
                rebar.AsPrime
                / (b * compressionHeight);

            double k =
                muS * alphaS2
                + muSPrime * alphaS1;

            if (k <= Tolerance)
                return 0.0;

            double value =
                k * k
                + 2.0 * k * rebar.aPrime / h0;

            if (value < 0.0)
                return 0.0;

            double xm =
                h0 * (
                    Math.Sqrt(value) - k);

            if (xm <= Tolerance ||
                xm >= h ||
                xm <= rebar.aPrime)
                return 0.0;

            return xm;
        }
    }
}
