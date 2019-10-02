using System.Linq;

namespace MatrixUWP.Extensions
{
    static class CopyExtensions
    {
        public static void CopyTo<T>(this T source, T target, params string[] surpass)
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
