using System;
using dcvietAdapter.Etabs;

namespace dcvietTests.ETABS
{
    internal static class EtabsStoryTests
    {
        public static void Run(EtabsModelAdapter model)
        {
            Console.WriteLine();
            Console.WriteLine("[TEST] STORY");
            Console.WriteLine("----------------------------------------");

            try
            {
                var stories = model.Stories.GetAll();

                Console.WriteLine(
                    "Story count : " +
                    stories.Count);

                foreach (var story in stories)
                {
                    Console.WriteLine(
                        $"{story.Id} | " +
                        $"{story.Name} | " +
                        $"Z={story.Elevation:F3} | " +
                        $"H={story.Height:F3} | " +
                        $"Master={story.IsMasterStory}");
                }

                Console.WriteLine("[OK] STORY");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[FAIL] STORY");
                Console.WriteLine(ex);
            }
        }
    }
}