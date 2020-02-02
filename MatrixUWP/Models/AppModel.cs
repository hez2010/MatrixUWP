#nullable enable
using MatrixUWP.Models.Config;
using MatrixUWP.Shared.Utils;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Media.Animation;

namespace MatrixUWP.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Parameter type</typeparam>
    static class AppModel
    {
        public static Action<string>? ShowMessage { get; set; }
        /// <summary>
        /// PageType, Parameter, TransitionInfo?
        /// </summary>
        public static Action<Type, object, NavigationTransitionInfo?>? NavigateToPage { get; set; }
        /// <summary>
        /// Culture strings resource
        /// </summary>
        internal static readonly ResourceLoader CultureResource = ResourceLoader.GetForCurrentView();

        internal static readonly Configuration AppConfiguration = new Configuration();

        private static readonly MatrixJsonHttpRequestBuilder MatrixHttpClientBuilder = new MatrixJsonHttpRequestBuilder();

        internal static MatrixJsonHttpRequestClient MatrixHttpClient = MatrixHttpClientBuilder.Build();
    }
}
