using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
