// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.Addons.Bootstrapper;

namespace Dapplo.CaliburnMicro.Dapp
{
    /// <summary>
    /// Extension(s) for ApplicationConfigBuilder
    /// </summary>
    public static class ApplicationConfigBuilderExtensions
    {
        /// <summary>
        /// Configure the ApplicationConfig to use Dapplo.CaliburnMicro by adding the right stuff
        /// </summary>
        /// <param name="applicationConfigBuilder">ApplicationConfigBuilder</param>
        /// <returns>ApplicationConfigBuilder</returns>
        public static ApplicationConfigBuilder WithCaliburnMicro(this ApplicationConfigBuilder applicationConfigBuilder)
        {
            // Load the Dapplo.CaliburnMicro.* assemblies
            applicationConfigBuilder.WithAssemblyPatterns("Dapplo.CaliburnMicro*");
            return applicationConfigBuilder;
        }
    }
}
