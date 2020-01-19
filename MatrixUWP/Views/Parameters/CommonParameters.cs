#nullable enable
﻿using MatrixUWP.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace MatrixUWP.Views.Parameters
{
    class CommonParameters : INotifyPropertyChanged
    {
        public Action<UserDataModel> UpdateUserData { get; set; }
        public UserDataModel UserData { get; set; }
        public Action<string> ShowMessage { get; set; }
        /// <summary>
        /// PageType, ParameterType, Parameter, TransitionInfo?
        /// </summary>
        public Action<Type, Type, object, NavigationTransitionInfo?> NavigateToPage { get; set; }

        public CommonParameters(CommonParameters param)
        {
            UpdateUserData = param.UpdateUserData;
            UserData = param.UserData;
            ShowMessage = param.ShowMessage;
            NavigateToPage = param.NavigateToPage;
        }

        public CommonParameters(Action<UserDataModel> updateUserData, UserDataModel userData,
            Action<string> showMessage, Action<Type, Type, object, NavigationTransitionInfo?> navigateToPage)
        {
            UpdateUserData = updateUserData;
            UserData = userData;
            ShowMessage = showMessage;
            NavigateToPage = navigateToPage;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
