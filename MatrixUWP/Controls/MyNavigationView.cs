#nullable enable
﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace MatrixUWP.Controls
{
    public sealed class MyNavigationView : NavigationView
    {
        public Visibility HeaderVisibility { get; set; }
        public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register(nameof(HeaderVisibility), typeof(Visibility), typeof(NavigationView), new PropertyMetadata(Visibility.Visible));

        public MyNavigationView()
        {
            DefaultStyleKey = typeof(MyNavigationView);
        }
    }
}
