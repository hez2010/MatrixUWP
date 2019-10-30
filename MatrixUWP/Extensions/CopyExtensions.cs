using System.Linq;

namespace MatrixUWP.Extensions
{
    static class CopyExtensions
    {
        public static void CopyTo(this object source, object target, params string[] suppress)
        {
            var targetProps = target.GetType().GetProperties();
            var sourceProps = source.GetType().GetProperties();
            foreach (var i in targetProps)
            {
                if (suppress?.Contains(i.Name) ?? false) continue;
                var sourceProp = sourceProps.FirstOrDefault(j => j.Name == i.Name);
                if (sourceProp != null && i.CanWrite && i.CanWrite)
                {
                    var value = sourceProp.GetValue(source);
                    i.SetValue(target, value);
                }
            }
        }
    }
}
