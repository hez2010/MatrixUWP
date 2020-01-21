#nullable enable
using System;
using System.Threading.Tasks;

namespace MatrixUWP.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<U> Then<T, U>(this Task<T> task, Func<T, Task<U>> then)
        {
            var result = await task;
            return await then(result);
        }

        public static async ValueTask<U> Then<T, U>(this ValueTask<T> task, Func<T, ValueTask<U>> then)
        {
            var result = await task;
            return await then(result);
        }
    }
}
