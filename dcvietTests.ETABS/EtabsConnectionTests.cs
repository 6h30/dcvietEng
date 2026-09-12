using System;
using dcvietAdapter.Etabs;

namespace dcvietTests.ETABS
{
    internal static class EtabsConnectionTests
    {
        public static EtabsConnection Connection { get; private set; }

        public static void Run()
        {
            Console.WriteLine();
            Console.WriteLine("[TEST] CONNECTION");
            Console.WriteLine("--------------------------------");

            try
            {
                Connection = new EtabsConnection();

                int ret = Connection.ConnectRunning();

                Console.WriteLine(
                    "ConnectRunning : " + ret);

                if (ret != 0)
                {
                    Console.WriteLine("[FAIL] CONNECTION");
                    return;
                }

                Console.WriteLine("[OK] CONNECTION");
                Console.WriteLine(
                    "IsConnected : " +
                    Connection.IsConnected);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[FAIL] CONNECTION");
                Console.WriteLine(ex);
            }
        }
    }
}