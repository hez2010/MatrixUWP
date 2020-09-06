#nullable enable
using Exceptionless;
using MatrixUWP.Models;
#if !DEBUG
using Exceptionless.Plugins;
#endif
using MatrixUWP.Views;
using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MatrixUWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
            EnteredBackground += OnEnteredBackground;
            LeavingBackground += OnLeavingBackground;
            SetupExceptionless();
        }

        private void SetupExceptionless()
        {
            var client = ExceptionlessClient.Default;
            UnhandledException += (sender, args) =>
            {
#if !DEBUG
                var contextData = new ContextData();
                contextData.MarkAsUnhandledError();
                contextData.SetSubmissionMethod("App_UnhandledException");
                args.Exception.ToExceptionless(contextData, client).Submit();
                client.ProcessQueue();
#endif
#if FAIL_ON_DEBUG
                Debug.Fail(args.Message);
                args.Handled = true;
#endif
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                if (!(args.ExceptionObject is Exception ex)) return;
#if !DEBUG
                var contextData = new ContextData();
                contextData.MarkAsUnhandledError();
                contextData.SetSubmissionMethod("AppDomain_UnhandledException");
                ex.ToExceptionless(contextData, client).Submit();
                client.ProcessQueue();
#endif
#if FAIL_ON_DEBUG
                Debug.Fail(ex.Message);
#endif
            };
            client.Startup("AjRPnkPtLOd7mZNKb1XU4yuxYqhS5Xe247IX07w5");
        }

        private void OnEnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
        }

        private void OnLeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
        }

        public static bool CanAccessBackground = true;

        protected override void OnActivated(IActivatedEventArgs e)
        {
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (!(Window.Current.Content is Frame rootFrame))
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e is ToastNotificationActivatedEventArgs args)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(Layout), null);
                }
                // Ensure the current window is active
                // TODO: handling toast action

                var coreView = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView();
                coreView.TitleBar.ExtendViewIntoTitleBar = true;

                var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
                var titleBar = appView.TitleBar;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

                Window.Current.Activate();
            }
        }

        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            // TODO: handling background toast action
        }

        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (!(Window.Current.Content is Frame rootFrame))
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(Layout), e.Arguments);
                }
                // Ensure the current window is active

                var coreView = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView();
                coreView.TitleBar.ExtendViewIntoTitleBar = true;

                var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
                var titleBar = appView.TitleBar;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e) => throw new Exception("Failed to load Page " + e.SourcePageType.FullName);

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
