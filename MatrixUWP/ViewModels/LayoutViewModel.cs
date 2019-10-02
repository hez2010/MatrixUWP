using MatrixUWP.Models;
using System;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace MatrixUWP.ViewModels
{
    class LayoutViewModel : INotifyPropertyChanged
    {
        public LayoutViewModel()
        {
            this.messageTimer.Tick += Timer_Tick;
        }

        private bool showMessage;
        private string message = "";
        private readonly DispatcherTimer messageTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };

        public UserDataModel UserData { get; } = new UserDataModel();
        public bool ShowMessage
        {
            get => showMessage;
            set
            {
                showMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowMessage)));
                if (showMessage) messageTimer.Start();
            }
        }
        public string Message
        {
            get => message;
            set
            {
                message = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            this.ShowMessage = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
