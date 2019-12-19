using System;
using System.Threading;
using System.Threading.Tasks;

namespace Numaka.Common
{
    /// <summary>
    /// Async helper
    /// </summary>
    public static class AsyncHelper
    {
        private static readonly TaskFactory TaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

        /// <summary>
        /// Run a task synchronously without blocking
        /// </summary>
        /// <param name="func"></param>
        public static void RunSync(Func<Task> func) => TaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();

        /// <summary>
        /// Run a task synchronously without blocking
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func) => TaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
    }
}