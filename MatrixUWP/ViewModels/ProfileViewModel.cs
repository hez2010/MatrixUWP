#nullable enable
using MatrixUWP.Models.User;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixUWP.ViewModels
{
    internal class ProfileViewModel : INotifyPropertyChanged
    {
        private bool loading;
        private UserDataModel? userData;

        public UserDataModel? UserData
        {
            get => userData;
            set
            {
                userData = value;
                OnPropertyChanged();
            }
        }

        public bool Loading
        {
            get => loading;
            set
            {
                loading = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
