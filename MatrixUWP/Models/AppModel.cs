#nullable enable
using MatrixUWP.Models.Config;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Media.Animation;

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
        /// PageType, Parameter, NaviMenuIndex
        /// </summary>
        public static Action<Type, object?, int>? NavigateToPage { get; set; }

        internal static readonly Configuration AppConfiguration = new Configuration();
    }
}
