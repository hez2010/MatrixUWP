#nullable enable
﻿using MatrixUWP.Extensions;
using MatrixUWP.Models.User;
using MatrixUWP.ViewModels;
using MatrixUWP.Views.General.Course;
using MatrixUWP.Views.Parameters;
using MatrixUWP.Views.Parameters.Course;
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
            InitializeComponent();

            Window.Current.SetTitleBar(MyTitleBar);
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;

            NaviContent.Navigated += NaviContent_Navigated;
            // Navigate to Home
            NavigateToPage(HomeNaviPage, false, NaviMenu.PaneDisplayMode);
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
        private (Type? Page, object? Parameter) GetTargetNaviInfo(string naviPageName, bool isSettingsPage)
        {
            // Get target page
            var page = (naviPageName, isSettingsPage) switch
            {
                (_, true) => typeof(Settings),
                (nameof(HomeNaviPage), _) => typeof(General.Home),
                (nameof(CourseNaviPage), _) => typeof(General.Course.Course),
                (nameof(ExamNaviPage), _) => typeof(General.Exam),
                (nameof(LibraryNaviPage), _) => typeof(General.Library),
                (nameof(MessagesNaviPage), _) => typeof(General.Messages),
                (nameof(ProfileNaviPage), _) => typeof(Account.Profile),
                (nameof(ManualNaviPage), _) => typeof(Help.Manual),
                (nameof(FeedbackNaviPage), _) => typeof(Help.Feedback),
                _ => null
            };

            UserModel.CurrentUser?.CopyTo(viewModel.UserData);
            var commonParameter =
                new CommonParameters(UpdateUserData, viewModel.UserData, ShowMessage, NavigateToPage);

            // Get parameters needed
            var parameter = (naviPageName, isSettingsPage) switch
            {
                (nameof(HomeNaviPage), _) => new HomeParameters(commonParameter),
                (nameof(ProfileNaviPage), _) => new ProfileParameters(commonParameter),
                (nameof(CourseNaviPage), _) => new CourseParameters(commonParameter),
                _ => commonParameter
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

            // Navigate to page
            NaviContent.Navigate(targetInfo.Page, targetInfo.Parameter, transition);

            // Set current selected item for navimenu
            NaviMenu.SelectedItem = naviItem;
            lastSelectedItemIndex = index;
        }

        private void NavigateToPage(Type pageType, Type parameterType, object parameter, NavigationTransitionInfo? transitionInfo)
        {
            var param = parameterType.GetConstructor(new[] { typeof(CommonParameters) })
                .Invoke(new[] { new CommonParameters(UpdateUserData, viewModel.UserData, ShowMessage, NavigateToPage) });
            parameter.CopyTo(param);
            NaviContent.Navigate(pageType, param, transitionInfo ?? new DrillInNavigationTransitionInfo());
        }

        private void UpdateUserData(UserDataModel userData)
        {
            userData.CopyTo(viewModel.UserData);
            if (!(userData?.SignedIn ?? false))
            {
                NavigateToPage(HomeNaviPage, false, NaviMenu.PaneDisplayMode);
                // clear all states
                navimenuNaviHistory.Clear();
                NaviContent.BackStack.Clear();
                CourseAssignments.LastCourseId = -1;
            }
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
    }
}
