using System;
using dcvietAdapter.Etabs;

namespace dcvietTests.ETABS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("       DCVIET ETABS ADAPTER TEST");
            Console.WriteLine("========================================");

            EtabsConnection connection = null;

            try
            {
                // =========================================
                // 1. CONNECTION
                // =========================================

                Console.WriteLine();
                Console.WriteLine("[1] CONNECTION");

                connection = new EtabsConnection();

                int ret = connection.ConnectRunning();

                if (ret != 0)
                {
                    Console.WriteLine(
                        "[FAIL] ConnectRunning()");

                    Console.WriteLine(
                        "Return code: " + ret);

                    return;
                }

                Console.WriteLine(
                    "[OK] ETABS CONNECTION");

                // =========================================
                // 2. MODEL
                // =========================================

                Console.WriteLine();
                Console.WriteLine("[2] MODEL");

                EtabsModelAdapter model =
                    new EtabsModelAdapter(connection);

                Console.WriteLine(
                    "[OK] ETABS MODEL ADAPTER");

                // =========================================
                // 3. INDIVIDUAL TESTS
                // =========================================

                Console.WriteLine();
                Console.WriteLine("[3] RUN ADAPTER TESTS");

                EtabsModelTests.Run(model);

                EtabsStoryTests.Run(model);

                EtabsPointTests.Run(model);

                EtabsSectionTests.Run(model);

                EtabsBeamTests.Run(model);

                EtabsResultTests.Run(model);

                // =========================================
                // 4. DONE
                // =========================================

                Console.WriteLine();
                Console.WriteLine("========================================");
                Console.WriteLine("       ALL ETABS TESTS COMPLETED");
                Console.WriteLine("========================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("========================================");
                Console.WriteLine("             TEST ERROR");
                Console.WriteLine("========================================");

                Console.WriteLine(ex);
            }
            finally
            {
                if (connection != null)
                    connection.Dispose();
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}