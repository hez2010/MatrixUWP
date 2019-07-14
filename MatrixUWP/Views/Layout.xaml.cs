using MatrixUWP.Controls;
using MatrixUWP.Extensions;
using MatrixUWP.Models;
using MatrixUWP.ViewModels;
using MatrixUWP.Views.Parameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace MatrixUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Layout : Page
    {
        private readonly LayoutViewModel viewModel = new LayoutViewModel();

        public Layout()
        {
            this.InitializeComponent();

            // Set up title bar style
            Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            Window.Current.SetTitleBar(MyTitleBar);

            // Register BackRequest handler
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;

            // Navigate to Home
            NavigateToPage(HomeNaviPage, false, NaviMenu.PaneDisplayMode);
        }

        /// <summary>
        /// Get target navi page and parameter
        /// </summary>
        /// <param name="naviPageName"></param>
        /// <param name="isSettingsPage"></param>
        /// <returns></returns>
        private (Type? Page, object? Parameter) GetTargetNaviInfo(string naviPageName, bool isSettingsPage)
        {
            // Get target page
            var page = (naviPageName, isSettingsPage) switch
            {
                (_, true) => typeof(Settings),
                ("HomeNaviPage", _) => typeof(General.Home),
                ("CourseNaviPage", _) => typeof(General.Course),
                ("ExamNaviPage", _) => typeof(General.Exam),
                ("LibraryNaviPage", _) => typeof(General.Library),
                ("MessagesNaviPage", _) => typeof(General.Messages),
                ("ProfileNaviPage", _) => typeof(Account.Profile),
                ("ManualNaviPage", _) => typeof(Help.Manual),
                ("FeedbackNaviPage", _) => typeof(Help.Feedback),
                _ => null
            };

            // Get parameters needed
            var parameter = (naviPageName, isSettingsPage) switch
            {
                ("HomeNaviPage", _) => new HomeParameters { UpdateUserData = UpdateUserData, UserData = viewModel.UserData, ShowMessage = ShowMessage },
                ("ProfileNaviPage", _) => new ProfileParameters {  UpdateUserData = UpdateUserData, UserData = viewModel.UserData, ShowMessage = ShowMessage },
                _ => new object()
            };

            return (page, parameter);
        }

        /// <summary>
        /// The last selected index of navimenu
        /// </summary>
        private int lastSelectedItemIndex = -1;
        /// <summary>
        /// Page navi history
        /// </summary>
        private readonly Stack<int> navimenuNaviHistory = new Stack<int>();
        private void NavigateToPage(object naviItem, bool isSettingsPage, NavigationViewPaneDisplayMode paneDisplayMode)
        {
            if (!(naviItem is NavigationViewItem item)) return;

            var targetInfo = GetTargetNaviInfo(item.Name, isSettingsPage);
            if (targetInfo.Page == null || targetInfo.Page == NaviContent.Content?.GetType()) return;

            // Get current selected index of navimenu
            var index = NaviMenu.MenuItems.IndexOf(item);
            if (isSettingsPage)
            {
                item.Content = App.CultureResource.GetString("NaviMenu_Item_Settings/Content");
                index = NaviMenu.MenuItems.Count;
            }

            // Set up page transition effect base on pane display mode
            NavigationTransitionInfo transition;
            if (paneDisplayMode == NavigationViewPaneDisplayMode.Top)
            {
                transition = new SlideNavigationTransitionInfo
                {
                    Effect = lastSelectedItemIndex <= index ? SlideNavigationTransitionEffect.FromRight : SlideNavigationTransitionEffect.FromLeft
                };
            }
            else transition = new DrillInNavigationTransitionInfo();

            // Update navi history and last selected index of navimenu
            if (lastSelectedItemIndex != -1) navimenuNaviHistory.Push(lastSelectedItemIndex);
            lastSelectedItemIndex = index;

            // Navigate to page
            NaviContent.Navigate(targetInfo.Page, targetInfo.Parameter, transition);

            // Set current selected item for navimenu
            NaviMenu.SelectedItem = naviItem;
        }

        private void UpdateUserData(UserDataModel userData)
        {
            userData.CopyTo(this.viewModel.UserData);
            if (!(userData?.SignedIn ?? false))
            {
                navimenuNaviHistory.Clear();
                NavigateToPage(HomeNaviPage, false, NaviMenu.PaneDisplayMode);
            }
        }

        private void ShowMessage(string message)
        {
            this.viewModel.Message = message;
            this.viewModel.ShowMessage = true;
        }

        private void NaviMenu_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            NavigateToPage(args.InvokedItemContainer, args.IsSettingsInvoked, sender.PaneDisplayMode);
        }

        private bool NavigateBack()
        {
            if (NaviContent.CanGoBack)
            {
                NaviContent.GoBack();
                if (navimenuNaviHistory.TryPop(out var index))
                {
                    if (index != NaviMenu.MenuItems.Count) NaviMenu.SelectedItem = NaviMenu.MenuItems[index];
                    else NaviMenu.SelectedItem = NaviMenu.SettingsItem;
                    lastSelectedItemIndex = index;
                }
                return true;
            }
            return false;
        }
        private void App_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            e.Handled = NavigateBack();
        }

        private void NaviMenu_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            NavigateBack();
        }
    }
}
