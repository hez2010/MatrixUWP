using System;
using System.Runtime.CompilerServices;
using Windows.UI.Core;

namespace MatrixUWP.Extensions
{
    /// <summary>
    /// This class provides static methods helper for executing code in UI thread of the main window.
    /// </summary>
    public static class DispatcherHelper
    {
        /// <summary>
        /// This struct represents an awaitable dispatcher.
        /// </summary>
        public struct DispatcherPriorityAwaitable
        {
            private readonly CoreDispatcher dispatcher;
            private readonly CoreDispatcherPriority priority;

            internal DispatcherPriorityAwaitable(CoreDispatcher dispatcher, CoreDispatcherPriority priority)
            {
                this.dispatcher = dispatcher;
                this.priority = priority;
            }

            public DispatcherPriorityAwaiter GetAwaiter() => new DispatcherPriorityAwaiter(this.dispatcher, this.priority);
        }

        /// <summary>
        /// This struct represents the awaiter of a dispatcher.
        /// </summary>
        public struct DispatcherPriorityAwaiter : INotifyCompletion
        {
            private readonly CoreDispatcher dispatcher;
            private readonly CoreDispatcherPriority priority;

            public bool IsCompleted => false;

            internal DispatcherPriorityAwaiter(CoreDispatcher dispatcher, CoreDispatcherPriority priority)
            {
                this.dispatcher = dispatcher;
                this.priority = priority;
            }

            public void GetResult() { }

            public async void OnCompleted(Action continuation)
            {
                await this.dispatcher.RunAsync(this.priority, new DispatchedHandler(continuation));
            }
        }

        /// <summary>
        /// Yield and allow UI update during tasks.
        /// </summary>
        /// <param name="dispatcher">Dispatcher of a thread to yield</param>
        /// <param name="priority">Dispatcher execution priority, default is low</param>
        public static DispatcherPriorityAwaitable YieldAsync(this CoreDispatcher dispatcher, CoreDispatcherPriority priority = CoreDispatcherPriority.Low) => new DispatcherPriorityAwaitable(dispatcher, priority);
    }

}
