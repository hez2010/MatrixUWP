using MatrixUWP.Models;
using MatrixUWP.Utils;
using System;
using System.ComponentModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MatrixUWP.ViewModels
{
    class ProfileViewModel : INotifyPropertyChanged
    {
        private bool loading;
        private UserDataModel userData = new UserDataModel();

        public UserDataModel UserData
        {
            get => userData;
            set
            {
                this.userData = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserData)));
            }
        }

        public bool Loading
        {
            get => loading;
            set
            {
                loading = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Loading)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
