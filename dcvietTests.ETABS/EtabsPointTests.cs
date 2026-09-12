using System;
using dcvietAdapter.Etabs;

namespace dcvietTests.ETABS
{
    internal static class EtabsPointTests
    {
        public static void Run(
            EtabsModelAdapter model)
        {
            Console.WriteLine();
            Console.WriteLine("[TEST] POINT");
            Console.WriteLine("----------------------------------------");

            try
            {
                var points =
                    model.Points.GetAll();

                Console.WriteLine(
                    $"Point count : {points.Count}");

                int displayCount =
                    Math.Min(
                        points.Count,
                        20);

                for (int i = 0;
                     i < displayCount;
                     i++)
                {
                    var point =
                        points[i];

                    Console.WriteLine(
                        $"Point : {point.Id}");

                    Console.WriteLine(
                        $"  Label : {point.Label}");

                    Console.WriteLine(
                        $"  XYZ   : " +
                        $"{point.X:F3}, " +
                        $"{point.Y:F3}, " +
                        $"{point.Z:F3}");

                    Console.WriteLine();
                }

                if (points.Count > displayCount)
                {
                    Console.WriteLine(
                        $"... " +
                        $"{points.Count - displayCount} " +
                        $"more points");
                }

                Console.WriteLine("[OK] POINT");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[FAIL] POINT");
                Console.WriteLine(ex);
            }
        }
    }
}