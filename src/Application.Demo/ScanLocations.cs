using System.Collections.Generic;
using System.IO;

namespace Application.Demo
{
    public static class ScanLocations
    {

        /// <summary>
        /// Helper method to create 
        /// </summary>
        /// <param name="platform">string with the platform</param>
        /// <param name="addons"></param>
        /// <returns></returns>
        public static IEnumerable<string> GenerateScanDirectories(string platform, params string[] addons)
        {
            var location = typeof(Startup).Assembly.Location;
            foreach (var addon in addons)
            {
                yield return Path.Combine(location, @"..\..\..\..\..\", addon, "bin",
#if DEBUG
                    "Debug",
#else
                    "Release",
#endif
                    platform
                    );
            }
        }
    }
}
