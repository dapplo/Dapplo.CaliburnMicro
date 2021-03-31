// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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