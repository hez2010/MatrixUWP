using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixUWP.Extensions
{
    static class CopyExtensions
    {
        public static void CopyTo<T>(this T source, T target, string[]? surpass = null)
        {
            var props = typeof(T).GetProperties();
            foreach (var i in props)
            {
                if (surpass?.Contains(i.Name) ?? false) continue;
                if (i.CanWrite && i.CanWrite)
                {
                    var value = i.GetValue(source);
                    i.SetValue(target, value);
                }
            }
        }
    }
}
