using System;
using dcvietAdapter.Etabs;

namespace dcvietTests.ETABS
{
    internal static class EtabsBeamTests
    {
        public static void Run(EtabsModelAdapter model)
        {
            Console.WriteLine();
            Console.WriteLine("[TEST] BEAM");
            Console.WriteLine("----------------------------------------");

            try
            {
                var beams = model.Beams.GetAll();

                Console.WriteLine(
                    "Beam count : " +
                    beams.Count);

                int displayCount =
                    Math.Min(beams.Count, 20);

                for (int i = 0;
                     i < displayCount;
                     i++)
                {
                    var beam = beams[i];

                    Console.WriteLine();
                    Console.WriteLine(
                        "Beam : " +
                        beam.Id);

                    Console.WriteLine(
                        "  Label   : " +
                        beam.Label);

                    Console.WriteLine(
                        "  Story   : " +
                        beam.Story);

                    Console.WriteLine(
                        "  Section : " +
                        beam.Section);

                    Console.WriteLine(
                        "  I Point : " +
                        beam.IPoint);

                    Console.WriteLine(
                        "  J Point : " +
                        beam.JPoint);

                    Console.WriteLine(
                        "  Length  : " +
                        beam.Length.ToString("F3"));
                }

                if (beams.Count > displayCount)
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        "... " +
                        (beams.Count - displayCount) +
                        " more beams");
                }

                Console.WriteLine();
                Console.WriteLine("[OK] BEAM");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[FAIL] BEAM");
                Console.WriteLine(ex);
            }
        }
    }
}