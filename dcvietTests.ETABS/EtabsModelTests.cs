using System;
using dcvietAdapter.Etabs;

namespace dcvietTests.ETABS
{
    internal static class EtabsModelTests
    {
        public static void Run(EtabsModelAdapter model)
        {
            Console.WriteLine();
            Console.WriteLine("[TEST] MODEL ADAPTER");
            Console.WriteLine("----------------------------------------");

            try
            {
                if (model == null)
                {
                    Console.WriteLine("[FAIL] Model is null.");
                    return;
                }

                Console.WriteLine(
                    "Beams   : " +
                    (model.Beams != null));

                Console.WriteLine(
                    "Points  : " +
                    (model.Points != null));

                Console.WriteLine(
                    "Stories : " +
                    (model.Stories != null));

                Console.WriteLine(
                    "Sections: " +
                    (model.Sections != null));

                Console.WriteLine(
                    "Results : " +
                    (model.Results != null));

                if (model.Beams == null ||
                    model.Points == null ||
                    model.Stories == null ||
                    model.Sections == null ||
                    model.Results == null)
                {
                    Console.WriteLine(
                        "[FAIL] One or more adapters are null.");

                    return;
                }

                Console.WriteLine(
                    "[OK] MODEL ADAPTER");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[FAIL] MODEL ADAPTER");
                Console.WriteLine(ex);
            }
        }
    }
}