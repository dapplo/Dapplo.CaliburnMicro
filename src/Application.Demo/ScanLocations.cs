//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2018 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.CaliburnMicro
// 
//  Dapplo.CaliburnMicro is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.CaliburnMicro is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.CaliburnMicro. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

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
