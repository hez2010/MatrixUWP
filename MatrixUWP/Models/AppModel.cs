#nullable enable
using MatrixUWP.Models.Config;
using System;

namespace MatrixUWP.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Parameter type</typeparam>
    internal static class AppModel
    {
        public static Action<string>? ShowMessage { get; set; }
        /// <summary>
        /// PageType, Parameter, NaviMenuIndex (-1 代表保持不变)
        /// </summary>
        public static Action<Type, object?, int>? NavigateToPage { get; set; }

        internal static readonly Configuration AppConfiguration = new Configuration();
    }
}
