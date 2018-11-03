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
using System.Linq;
using System.Text.RegularExpressions;
using Application.Demo;
using Dapplo.Addons.Bootstrapper.Resolving;
using Dapplo.CaliburnMicro.Menu;
using Dapplo.Config.Language;
using Dapplo.Log;
using Dapplo.Log.XUnit;
using Xunit;
using Xunit.Abstractions;

namespace Dapplo.Caliburn.Tests
{
    public class BasicTests
    {
        private static readonly LogSource Log = new LogSource();
        public BasicTests(ITestOutputHelper testOutputHelper)
        {
            LogSettings.RegisterDefaultLogger<XUnitLogger>(LogLevels.Verbose, testOutputHelper);
        }
        [Fact]
        public void Test_Language_Scan()
        {
            var languageConfig = LanguageConfigBuilder.Create()
                .WithSpecificDirectories(
                    ScanLocations.GenerateScanDirectories(
#if NET471
                    "net471",
#else
                    "netcoreapp3.0",
#endif
                        "Application.Demo",
                        "Application.Demo.Addon",
                        "Application.Demo.MetroAddon",
                        "Application.Demo.OverlayAddon").ToArray()
                )
                .BuildLanguageConfig();


            var filePattern = new Regex(languageConfig.FileNamePattern, RegexOptions.Compiled);

            var groupedFiles = FileLocations.Scan(languageConfig.SpecifiedDirectories, filePattern)
                .GroupBy(x => x.Item2.Groups["IETF"].Value)
                .ToDictionary(group => group.Key, group => group.Select(x => x.Item1)
                    .ToList());

            var foundFiles = FileLocations.Scan(languageConfig.SpecifiedDirectories, filePattern).Select(tuple => tuple.Item1).Select(Path.GetFileName).ToList();
            Assert.Contains("language_addon1-de-DE.xml", foundFiles);
            Assert.Contains("language_addon1-en-US.xml", foundFiles);
        }

        [Fact]
        public void Test_Tree()
        {
            var item1 = new MenuItem
            {
                Id = "A_1"
            };
            var item11 = new MenuItem
            {
                Id = "A_1_1",
                ParentId = "A_1"
            };
            var item12 = new MenuItem
            {
                Id = "A_1_2",
                ParentId = "A_1"
            };
            var item0 = new MenuItem
            {
                Id = "A_0"
            };
            var items = new List<IMenuItem> {
                item11, item1, item12, item0
            };

            bool wasA0AlreadySeen = false;

            var tree = items.CreateTree();
            foreach (var treeNode in tree)
            {
                if (treeNode.Id == "A_1")
                {
                    Assert.True(treeNode.ChildNodes.Count == 2);
                }
                if (treeNode.Id == "A_1")
                {
                    Assert.True(wasA0AlreadySeen);
                }
                if (treeNode.Id == "A_0")
                {
                    wasA0AlreadySeen = true;
                }
            }
        }
    }
}
