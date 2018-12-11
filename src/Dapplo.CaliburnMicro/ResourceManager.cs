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

using System;
using System.Linq;
using System.Windows;
using Dapplo.Addons;
using Dapplo.Log;

namespace Dapplo.CaliburnMicro
{
    /// <summary>
    /// This manages the XAML resources with styles etc for the application
    /// </summary>
    public class ResourceManager
    {
        private static readonly LogSource Log = new LogSource();

        private readonly IResourceProvider _resourceProvider;

        public ResourceManager(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
        }
        /// <summary>
        ///     Add a single ResourceDictionary for the supplied source
        ///     An example would be /Resources/Icons.xaml (which is in MahApps.Metro.Resources)
        /// </summary>
        /// <param name="source">Uri, e.g. /Resources/Icons.xaml or </param>
        public void AddResourceDictionary(Uri source)
        {
            if (source.Scheme == "pack" && !_resourceProvider.EmbeddedResourceExists(source))
            {
                Log.Warn().WriteLine("Resource {0} might not be available.", source);
            }
            if (Application.Current.Resources.MergedDictionaries.Any(x => x.Source == source))
            {
                return;
            }

            var resourceDictionary = new ResourceDictionary
            {
                Source = source
            };
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }

        /// <summary>
        ///     Remove a single ResourceDictionary for the supplied source
        ///     An example would be /Resources/Icons.xaml (which is in MahApps.Metro.Resources)
        /// </summary>
        /// <param name="source">Uri, e.g. /Resources/Icons.xaml or </param>
        public void DeleteResourceDictionary(Uri source)
        {
            foreach (var resourceDirectory in Application.Current.Resources.MergedDictionaries.ToList())
            {
                if (resourceDirectory.Source == source)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(resourceDirectory);
                }
            }
        }
    }
}
