using System.Collections.Generic;
using System.IO;

namespace Application.Demo
{
    public static class ScanLocations
    {

        /// <summary>
        /// Helper method to create 
        /// </summary>
        /// <param name="addons"></param>
        /// <returns></returns>
        public static IEnumerable<string> GenerateScanDirectories(params string[] addons)
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
#if !NET461
                    "net461"
#else
                    "netcoreapp3.0"
#endif
                    );
            }
        }
    }
}
