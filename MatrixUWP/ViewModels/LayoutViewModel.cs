using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MatrixUWP.ViewModels
{
    class LayoutViewModel : INotifyPropertyChanged
    {
        private bool isSignedIn;

        public bool IsSignedIn
        {
            get => isSignedIn;
            set
            {
                isSignedIn = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NaviItemVisibility)));
            }
        }

        public Visibility NaviItemVisibility => IsSignedIn ? Visibility.Visible : Visibility.Collapsed;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
