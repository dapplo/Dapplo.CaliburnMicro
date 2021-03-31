// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Caliburn.Micro;
using Dapplo.Log;

namespace Dapplo.CaliburnMicro
{
    /// <summary>
    ///     A logger for Caliburn
    /// </summary>
    public class CaliburnLogger : ILog
    {
        private readonly LogSource _log;

        /// <summary>
        ///     The constructor is called from the LogManager.GetLog function with the type to log for
        /// </summary>
        /// <param name="type"></param>
        public CaliburnLogger(Type type)
        {
            _log = new LogSource(type);
        }

        /// <summary>
        ///     Log an error
        /// </summary>
        /// <param name="exception"></param>
        public void Error(Exception exception)
        {
            _log.Error().WriteLine(exception);
        }

        /// <summary>
        ///     Log information, this is actually reduced to the Dapplo-Level debug as Caliburn speaks a lot!
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void Info(string format, params object[] args)
        {
            // Pre-format the message, otherwise we get problems with dependency objects etc
            _log.Debug().WriteLine(format, args);
        }

        /// <summary>
        ///     Log warning
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void Warn(string format, params object[] args)
        {
            // Pre-format the message, otherwise we get problems with dependency objects etc
            _log.Warn().WriteLine(format, args);
        }
    }
}