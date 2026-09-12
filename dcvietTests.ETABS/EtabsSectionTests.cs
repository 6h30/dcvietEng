using System;
using dcvietAdapter.Etabs;

namespace dcvietTests.ETABS
{
    internal static class EtabsSectionTests
    {
        public static void Run(EtabsModelAdapter model)
        {
            Console.WriteLine();
            Console.WriteLine("[TEST] SECTION");
            Console.WriteLine("----------------------------------------");

            try
            {
                var sections =
                    model.Sections.GetAll();

                Console.WriteLine(
                    "Section count : " +
                    sections.Count);

                int displayCount =
                    Math.Min(sections.Count, 20);

                for (int i = 0;
                     i < displayCount;
                     i++)
                {
                    var section = sections[i];

                    Console.WriteLine(
                        $"{section.Id} | " +
                        $"Material={section.Material} | " +
                        $"Shape={section.Shape} | " +
                        $"B={section.Width:F3} | " +
                        $"H={section.Depth:F3}");
                }

                if (sections.Count > displayCount)
                {
                    Console.WriteLine(
                        "... " +
                        (sections.Count - displayCount) +
                        " more sections");
                }

                Console.WriteLine("[OK] SECTION");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[FAIL] SECTION");
                Console.WriteLine(ex);
            }
        }
    }
}