using System;
using System.Runtime.CompilerServices;
using Windows.UI.Core;

namespace MatrixUWP.Extensions
{
    static class DispatcherExtensions
    {
        public static DispatcherPriorityAwaitable Yield(this CoreDispatcher dispatcher) => Yield(dispatcher, CoreDispatcherPriority.Low);
        public static DispatcherPriorityAwaitable Yield(this CoreDispatcher dispatcher, CoreDispatcherPriority priority) => new DispatcherPriorityAwaitable(dispatcher, priority);
    }

    struct DispatcherPriorityAwaitable
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

    struct DispatcherPriorityAwaiter : INotifyCompletion
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

        public async void OnCompleted(Action continuation) => await this.dispatcher.RunAsync(this.priority, new DispatchedHandler(continuation));
    }
}
