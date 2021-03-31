//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2020 Dapplo
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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private static readonly LogSource Log = new();
        private readonly ReaderWriterLockSlim _applicationResourcesReaderWriterLockSlim = new();
        private readonly ReaderWriterLockSlim _resourcesReaderWriterLockSlim = new();
        private readonly IResourceProvider _resourceProvider;
        private List<(Uri Source, int Order)> _resources = new();

        /// <summary>
        /// Supplies the list of Resources
        /// </summary>
        public IEnumerable<(Uri Source, int Order)> Resources => _resources.AsReadOnly();

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="resourceProvider">IResourceProvider</param>
        public ResourceManager(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
        }

        /// <summary>
        /// Internal method which applies the resources to the application
        /// </summary>
        private void ApplyResources()
        {
            try
            {
                _applicationResourcesReaderWriterLockSlim.EnterWriteLock();

                Application.Current.Resources.BeginInit();
                var mergedResources = Application.Current.Resources.MergedDictionaries;
                mergedResources.Clear();

                _resourcesReaderWriterLockSlim.EnterReadLock();
                foreach (var resource in _resources.OrderBy(resourceTuple => resourceTuple.Order).Select(resourceTuple => resourceTuple.Source))
                {
                    var resourceDictionary = new ResourceDictionary
                    {
                        Source = resource
                    };
                    mergedResources.Add(resourceDictionary);
                }
                Application.Current.Resources.EndInit();
            }
            finally
            {
                _applicationResourcesReaderWriterLockSlim.ExitWriteLock();
                _resourcesReaderWriterLockSlim.ExitReadLock();
            }
        }

        /// <summary>
        ///     Add a single ResourceDictionary for the supplied source
        ///     An example would be /Resources/Icons.xaml (which is in MahApps.Metro.Resources)
        /// </summary>
        /// <param name="source">Uri, e.g. /Resources/Icons.xaml or </param>
        /// <param name="order">int with the order used when applying</param>
        /// <param name="applyResources">bool true if the resources need to be applied</param>
        public void AddResourceDictionary(Uri source, int order = 0, bool applyResources = true)
        {
            if (source == null)
            {
                return;
            }
            if (source.Scheme == "pack" && !_resourceProvider.EmbeddedResourceExists(source))
            {
                Log.Warn().WriteLine("Resource {0} might not be available.", source);
            }

            try
            {
                _applicationResourcesReaderWriterLockSlim.EnterWriteLock();
                if (_resources.Any(resource => resource.Source == source))
                {
                    return;
                }
                _resources.Add((source, order));
            }
            finally
            {
                _applicationResourcesReaderWriterLockSlim.ExitWriteLock();
            }

            if (applyResources)
            {
                ApplyResources();
            }
        }

        /// <summary>
        ///     Add a multiple ResourceDictionaries
        ///     An example would be /Resources/Icons.xaml (which is in MahApps.Metro.Resources)
        /// </summary>
        /// <param name="sources">IEnumerable with tuples of Uri and int- Order</param>
        public void AddResourceDictionaries(IEnumerable<(Uri Source, int Order)> sources)
        {
            try
            {
                _applicationResourcesReaderWriterLockSlim.EnterWriteLock();
                foreach (var sourceTuple in sources)
                {
                    var source = sourceTuple.Source;
                    if (source.Scheme == "pack" && !_resourceProvider.EmbeddedResourceExists(source))
                    {
                        Log.Warn().WriteLine("Resource {0} might not be available.", source);
                    }
                    if (_resources.Any(resource => resource.Source == source))
                    {
                        continue;
                    }
                    _resources.Add(sourceTuple);
                }
            }
            finally
            {
                _applicationResourcesReaderWriterLockSlim.ExitWriteLock();
            }

            ApplyResources();
        }

        /// <summary>
        ///     Remove a single ResourceDictionary for the supplied source
        ///     An example would be /Resources/Icons.xaml (which is in MahApps.Metro.Resources)
        /// </summary>
        /// <param name="source">Uri, e.g. /Resources/Icons.xaml or </param>
        public void DeleteResourceDictionary(Uri source)
        {
            try
            {
                _applicationResourcesReaderWriterLockSlim.EnterReadLock();
                var newList = _resources.Where(resource => resource.Source != source).ToList();
                _resources = newList;
            }
            finally
            {
                _applicationResourcesReaderWriterLockSlim.ExitReadLock();
            }
            ApplyResources();
        }
    }
}
