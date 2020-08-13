#nullable enable
using MatrixUWP.Models;
using MatrixUWP.Models.User;
using MatrixUWP.Shared.Models;
using MatrixUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using NavigationView = Microsoft.UI.Xaml.Controls.NavigationView;
using NavigationViewItem = Microsoft.UI.Xaml.Controls.NavigationViewItem;

namespace MatrixUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Layout : Page
    {
        private readonly LayoutViewModel viewModel = new LayoutViewModel();
        private static bool loaded = false;
        private static int previousUserId = -1;
        private object? lastSelectedItem;

        public Layout()
        {
            AppModel.ShowMessage = ShowMessage;
            AppModel.NavigateToPage = NavigateToPage;

            InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
        }

        private void UserDataChanged()
        {
            var userData = UserModel.CurrentUser;
            if (previousUserId == userData.UserId) return;
            previousUserId = userData.UserId;
            if (userData.SignedIn) return;

            navimenuNaviHistory.Clear();
            lastSelectedItem = null;
            NavigateToPage(typeof(Home), null, 0);
            NaviContent.BackStack.Clear();
            Course.CourseAssignments.LastCourseId = -1;
        }

        /// <summary>
        /// Get target navi page and parameter
        /// </summary>
        /// <param name="naviPageTag"></param>
        /// <returns></returns>
        private Type? GetTargetNaviInfo(string? naviPageTag)
        {
            // Get target page
            var page = naviPageTag switch
            {
                "HomeNaviPage" => typeof(Home),
                "CourseNaviPage" => typeof(Course.Course),
                "MessagesNaviPage" => typeof(Messages),
                "ProfileNaviPage" => typeof(Profile),
                "HelpNaviPage" => typeof(Manual),
                "SettingsNaviPage" => typeof(Settings),
                _ => null
            };

            return page;
        }

        /// <summary>
        /// Page navi history
        /// </summary>
        private readonly Stack<object> navimenuNaviHistory = new Stack<object>();
        private void NavigateToPage(NavigationViewItem naviItem, object? parameters, NavigationTransitionInfo transInfo)
        {
            var targetInfo = GetTargetNaviInfo(naviItem.Tag as string);
            if (targetInfo == null || targetInfo == NaviContent.Content?.GetType()) return;

            // Navigate to page
            NaviContent.Navigate(targetInfo, parameters, transInfo);
        }

        private void NavigateToPage(Type pageType, object? parameter, int navimenuIndex)
        {
            if (navimenuIndex >= 0 && navimenuIndex < NaviMenu.MenuItems.Count)
            {
                NaviMenu.SelectedItem = NaviMenu.MenuItems[navimenuIndex];
            }
            NaviContent.Navigate(pageType, parameter, new DrillInNavigationTransitionInfo());
        }

        private void ShowMessage(string message)
        {
            viewModel.Message = message;
            viewModel.ShowMessage = true;
        }

        private bool NavigateBack()
        {
            if (NaviContent.CanGoBack)
            {
                NaviContent.GoBack();
                if (navimenuNaviHistory.TryPop(out var item))
                {
                    NaviMenu.SelectedItem = item;
                    lastSelectedItem = item;
                }
                return true;
            }
            return false;
        }

        private void App_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e) => e.Handled = NavigateBack();

        private void Page_Unloaded(object sender, RoutedEventArgs e) => UserModel.OnUserDataUpdate -= UserDataChanged;

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(Home), null, 0);
            UserModel.OnUserDataUpdate += UserDataChanged;

            if (!loaded)
            {
                loaded = true;
                viewModel.Loading = true;

                try
                {
                    var response = await UserModel.GetUserProfile();
                    if (response?.Status == StatusCode.OK)
                    {
                        UserModel.UpdateUserData(response.Data);
                    }
                }
                catch { /* ignored */ }
                viewModel.Loading = false;
            }
        }

        private void NaviContent_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (e.NavigationMode != Windows.UI.Xaml.Navigation.NavigationMode.Back)
            {
                if (lastSelectedItem != null)
                {
                    navimenuNaviHistory.Push(lastSelectedItem);
                }
                lastSelectedItem = NaviMenu.SelectedItem;
            }
        }

        private void NaviMenu_ItemInvoked(NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer is NavigationViewItem nvi && nvi.Tag != null)
            {
                NavigateToPage(nvi, null, args.RecommendedNavigationTransitionInfo);
            }
        }

        private void NaviMenu_BackRequested(NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewBackRequestedEventArgs args)
        {
            NavigateBack();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateBack();
        }

        private Visibility BindingAndToVisibility(bool left, bool right) 
            => (left && right) ? Visibility.Visible : Visibility.Collapsed;

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await UserModel.SignOutAsync();
                ShowMessage(result?.Message ?? "发生错误");
            }
            catch (Exception ex)
            {
                AppModel.ShowMessage?.Invoke(ex.Message);
#if FAIL_ON_DEBUG
                Debug.Fail(ex.Message, ex.StackTrace);
#endif
            }
            finally
            {
                viewModel.Loading = false;
            }
        }
    }
}
