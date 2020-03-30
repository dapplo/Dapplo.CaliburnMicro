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

using System.Threading.Tasks;
using System.Windows.Threading;

namespace Dapplo.CaliburnMicro.Extensions
{
    /// <summary>
    ///     Helper extensions for tasks
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        ///     Wait for the supplied task, copied from
        ///     <a
        ///         href="https://blogs.msdn.microsoft.com/pfxteam/2012/04/13/should-i-expose-synchronous-wrappers-for-asynchronous-methods/">
        ///         here
        ///     </a>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task">Task to wait for</param>
        /// <returns>the result</returns>
        public static T WaitWithNestedMessageLoop<T>(this Task<T> task)
        {
            var nested = new DispatcherFrame();
            task.ContinueWith(_ => nested.Continue = false, TaskScheduler.Default);
            Dispatcher.PushFrame(nested);
            return task.Result;
        }

        /// <summary>
        ///     Wait for the supplied task, copied from
        ///     <a
        ///         href="https://blogs.msdn.microsoft.com/pfxteam/2012/04/13/should-i-expose-synchronous-wrappers-for-asynchronous-methods/">
        ///         here
        ///     </a>
        /// </summary>
        /// <param name="task">Task to wait for</param>
        /// <returns>the result</returns>
        public static void WaitWithNestedMessageLoop(this Task task)
        {
            var nested = new DispatcherFrame();
            task.ContinueWith(_ => nested.Continue = false, TaskScheduler.Default);
            Dispatcher.PushFrame(nested);
            task.Wait();
        }
    }
}