using System;
using System.Threading;
using System.Threading.Tasks;

namespace Numaka.Common
{
    /// <summary>
    /// This is a hack to get around executing async code from non-async context. Only use it when there is no other approach available.
    /// See this article for more information...
    /// https://docs.microsoft.com/en-us/archive/msdn-magazine/2015/july/async-programming-brownfield-async-development#the-blocking-hack
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