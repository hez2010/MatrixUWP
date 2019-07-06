using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixUWP.Utils
{
    class TryHelper
    {
        public static async Task TryAsync(Func<Task> func)
        {
            try
            {
                await func();
            }
            catch (Exception ex)
            {
                ex.Log();
                throw;
            }
        }
        public static async Task<T> TryAsync<T>(Func<Task<T>> func)
        {
            try
            {
                var result = await func();
                return result;
            }
            catch (Exception ex)
            {
                ex.Log();
                throw;
            }
        }
        public static void Try(Action func)
        {
            try
            {
                func();
            }
            catch (Exception ex)
            {
                ex.Log();
                throw;
            }
        }
        public static T Try<T>(Func<T> func)
        {
            try
            {
                var result = func();
                return result;
            }
            catch (Exception ex)
            {
                ex.Log();
                throw;
            }
        }
    }
}
