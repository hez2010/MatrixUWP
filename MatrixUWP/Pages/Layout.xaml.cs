using MatrixUWP.Controls;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MatrixUWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Layout : Page
    {
        public Layout()
        {
            this.InitializeComponent();

            Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            NaviMenu.SelectedItem = HomePage;
        }

        private int lastSelectedItemIndex = 0;
        private void NavigateToPage(object itemObject, bool isSettingsPage, NavigationViewPaneDisplayMode paneDisplayMode)
        {
            if (!(itemObject is NavigationViewItem item)) return;
            var index = NaviMenu.MenuItems.IndexOf(item);
            if (isSettingsPage)
            {
                item.Content = App.CultureResource.GetString("NaviMenu_Item_Settings/Content");
                index = NaviMenu.MenuItems.Count;
            }

            NavigationTransitionInfo transition;
            if (paneDisplayMode == NavigationViewPaneDisplayMode.Top)
            {
                transition = new SlideNavigationTransitionInfo
                {
                    Effect = lastSelectedItemIndex <= index ? SlideNavigationTransitionEffect.FromRight : SlideNavigationTransitionEffect.FromLeft
                };
            }
            else transition = new DrillInNavigationTransitionInfo();
            lastSelectedItemIndex = index;

            NaviContent.Navigate((item.Name, isSettingsPage) switch
            {
                (_, true) => typeof(Pages.Settings),
                ("HomeNaviPage", _) => typeof(Pages.Generic.Home),
                _ => typeof(Pages.Generic.Home)
            }, null, transition);
        }

        private void NaviMenu_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            NavigateToPage(args.InvokedItem, args.IsSettingsInvoked, sender.PaneDisplayMode);
        }
        private void NaviMenu_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigateToPage(args.SelectedItem, args.IsSettingsSelected, sender.PaneDisplayMode);
        }
    }
}
