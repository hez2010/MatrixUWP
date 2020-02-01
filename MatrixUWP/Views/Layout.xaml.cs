#nullable enable
using MatrixUWP.Models;
using MatrixUWP.Models.User;
using MatrixUWP.ViewModels;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

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
            AppModel.ShowMessage = ShowMessage;
            AppModel.NavigateToPage = NavigateToPage;

            InitializeComponent();

            Window.Current.SetTitleBar(MyTitleBar);
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
        }

        private void UserData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!(sender is UserDataModel userData)) return;
            if (userData?.SignedIn ?? false) return;

            NavigateToPage(HomeNaviPage, false, NaviMenu.PaneDisplayMode);
            // clear all states
            navimenuNaviHistory.Clear();
            NaviContent.BackStack.Clear();
            Course.CourseAssignments.LastCourseId = -1;
        }

        private void NaviContent_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (e.NavigationMode == Windows.UI.Xaml.Navigation.NavigationMode.Back) return;
            // Update navi history and last selected index of navimenu
            if (lastSelectedItemIndex != -1) navimenuNaviHistory.Push(lastSelectedItemIndex);
        }

        /// <summary>
        /// Get target navi page and parameter
        /// </summary>
        /// <param name="naviPageName"></param>
        /// <param name="isSettingsPage"></param>
        /// <returns></returns>
        private Type? GetTargetNaviInfo(string naviPageName, bool isSettingsPage)
        {
            // Get target page
            var page = (naviPageName, isSettingsPage) switch
            {
                (_, true) => typeof(Settings),
                (nameof(HomeNaviPage), _) => typeof(Home),
                (nameof(CourseNaviPage), _) => typeof(Course.Course),
                (nameof(LibraryNaviPage), _) => typeof(Library),
                (nameof(MessagesNaviPage), _) => typeof(Messages),
                (nameof(ProfileNaviPage), _) => typeof(Profile),
                (nameof(ManualNaviPage), _) => typeof(Manual),
                _ => null
            };

            return page;
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
            if (targetInfo == null || targetInfo == NaviContent.Content?.GetType()) return;

            // Get current selected index of navimenu
            var index = NaviMenu.MenuItems.IndexOf(item);
            if (isSettingsPage)
            {
                item.Content = AppModel.CultureResource.GetString("NaviMenu_Item_Settings/Content");
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

            // Navigate to page
            NaviContent.Navigate(targetInfo, null, transition);

            // Set current selected item for navimenu
            NaviMenu.SelectedItem = naviItem;
            lastSelectedItemIndex = index;
        }

        private void NavigateToPage(Type pageType, object parameter, NavigationTransitionInfo? transitionInfo)
        {
            NaviContent.Navigate(pageType, parameter, transitionInfo ?? new DrillInNavigationTransitionInfo());
        }

        private void ShowMessage(string message)
        {
            viewModel.Message = message;
            viewModel.ShowMessage = true;
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
                    if (index == -1) return true;
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

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            viewModel.UserData.PropertyChanged -= UserData_PropertyChanged;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Navigate to Home
            NavigateToPage(HomeNaviPage, false, NaviMenu.PaneDisplayMode);
            viewModel.UserData.PropertyChanged += UserData_PropertyChanged;
        }
    }
}
