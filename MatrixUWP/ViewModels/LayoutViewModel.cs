﻿using MatrixUWP.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
                OnPropertyChanged();
                if (showMessage) messageTimer.Start();
            }
        }
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            this.ShowMessage = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
