using System;
using dcvietAdapter.Etabs;

namespace dcvietTests.ETABS
{
    internal static class EtabsResultTests
    {
        // ============================================================
        // RUN ALL RESULT TESTS
        // ============================================================

        public static void Run(EtabsModelAdapter model)
        {
            Console.WriteLine();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("        ETABS RESULT TESTS");
            Console.WriteLine("----------------------------------------");

            // --------------------------------------------------------
            // 1. FRAME FORCE
            // --------------------------------------------------------

            TestFrameForce(model);

            // --------------------------------------------------------
            // 2. JOINT DISPLACEMENT
            // --------------------------------------------------------

            TestJointDisplacement(model);

            Console.WriteLine();
            Console.WriteLine(
                "[OK] ETABS RESULT TESTS COMPLETED");
        }


        // ============================================================
        // FRAME FORCE TEST
        //
        // IMPORTANT:
        //
        // Trước khi chạy:
        //
        // 1. Mở ETABS
        // 2. Run Analysis
        // 3. Select đúng 1 BEAM trong ETABS
        //
        // Ví dụ:
        //
        // B464
        //
        // Code sẽ lấy:
        //
        // FrameForce(
        //     "All",
        //     SelectionElm,
        //     ...)
        //
        // giống VBA.
        // ============================================================

        private static void TestFrameForce(
            EtabsModelAdapter model)
        {
            Console.WriteLine();
            Console.WriteLine("[FRAME FORCE]");

            const string comboName = "ULS";

            Console.WriteLine(
                "Select ONE beam in ETABS.");

            Console.WriteLine(
                "Combo : " + comboName);

            try
            {
                // ====================================================
                // EtabsModelAdapter.Results
                //
                // Core chỉ biết IResultAdapter.
                //
                // GetSelectedFrameForces() là ETABS-specific,
                // nên cast sang EtabsResultAdapter.
                // ====================================================

                var resultAdapter =
                    model.Results as EtabsResultAdapter;

                if (resultAdapter == null)
                {
                    Console.WriteLine(
                        "[FRAME FORCE ERROR] " +
                        "EtabsResultAdapter is not available.");

                    return;
                }

                // ====================================================
                // GET FRAME FORCE FROM CURRENT ETABS SELECTION
                // ====================================================

                var results =
                    resultAdapter.GetSelectedFrameForces(
                        comboName);

                if (results == null)
                {
                    Console.WriteLine(
                        "[FRAME FORCE ERROR] " +
                        "Result is null.");

                    return;
                }

                Console.WriteLine(
                    "Number Results : " +
                    results.Count);

                // ====================================================
                // NO RESULT
                // ====================================================

                if (results.Count == 0)
                {
                    Console.WriteLine(
                        "[FRAME FORCE ERROR] " +
                        "No FrameForce results returned.");

                    Console.WriteLine(
                        "Check:");

                    Console.WriteLine(
                        "  1. A beam is selected in ETABS.");

                    Console.WriteLine(
                        "  2. Analysis has been run.");

                    Console.WriteLine(
                        "  3. Combo 'ULS' exists.");

                    return;
                }

                // ====================================================
                // PRINT RESULTS
                // ====================================================

                Console.WriteLine();

                foreach (var item in results)
                {
                    Console.WriteLine(
                        $"Object={item.ObjectId} | " +
                        $"Element={item.ElementId} | " +
                        $"Station={item.Station:F3} | " +
                        $"LoadCase={item.LoadCase} | " +
                        $"StepType={item.StepType} | " +
                        $"Step={item.StepNumber:F0} | " +
                        $"P={item.P:F3} | " +
                        $"V2={item.V2:F3} | " +
                        $"V3={item.V3:F3} | " +
                        $"T={item.T:F3} | " +
                        $"M2={item.M2:F3} | " +
                        $"M3={item.M3:F3}");
                }

                Console.WriteLine();

                Console.WriteLine(
                    "[OK] Frame Force API returned data.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "[FRAME FORCE ERROR]");

                Console.WriteLine(
                    ex);
            }
        }


        // ============================================================
        // JOINT DISPLACEMENT TEST
        // ============================================================

        private static void TestJointDisplacement(
            EtabsModelAdapter model)
        {
            Console.WriteLine();
            Console.WriteLine("[JOINT DISPLACEMENT]");

            const string pointId = "1";
            const string comboName = "ULS";

            Console.WriteLine(
                "Point : " + pointId);

            Console.WriteLine(
                "Combo : " + comboName);

            try
            {
                var results =
                    model.Results.GetJointDisplacements(
                        pointId,
                        comboName);

                if (results == null)
                {
                    Console.WriteLine(
                        "[JOINT DISPLACEMENT ERROR] " +
                        "Result is null.");

                    return;
                }

                Console.WriteLine(
                    "Number Results : " +
                    results.Count);

                if (results.Count == 0)
                {
                    Console.WriteLine(
                        "[JOINT DISPLACEMENT ERROR] " +
                        "No displacement results returned.");

                    return;
                }

                foreach (var item in results)
                {
                    Console.WriteLine(
                        $"LoadCase={item.LoadCase} | " +
                        $"StepType={item.StepType} | " +
                        $"Step={item.StepNumber:F0} | " +
                        $"U1={item.U1:F6} | " +
                        $"U2={item.U2:F6} | " +
                        $"U3={item.U3:F6} | " +
                        $"R1={item.R1:F6} | " +
                        $"R2={item.R2:F6} | " +
                        $"R3={item.R3:F6}");
                }

                Console.WriteLine();

                Console.WriteLine(
                    "[OK] Joint Displacement API returned data.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "[JOINT DISPLACEMENT ERROR]");

                Console.WriteLine(
                    ex);
            }
        }
    }
}