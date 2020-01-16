using MatrixUWP.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
