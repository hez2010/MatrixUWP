using MatrixUWP.Models;
using System;
using Windows.UI.Xaml.Controls;

namespace MatrixUWP.Views.Parameters
{
    class CommonParameters
    {
        public Action<UserDataModel> UpdateUserData { get; set; }
        public UserDataModel UserData { get; set; }
        public Action<string> ShowMessage { get; set; }
        public Frame NaviContent { get; set; }

        public CommonParameters(CommonParameters param)
        {
            this.UpdateUserData = param.UpdateUserData;
            this.UserData = param.UserData;
            this.ShowMessage = param.ShowMessage;
            this.NaviContent = param.NaviContent;
        }

        public CommonParameters(Action<UserDataModel> updateUserData, UserDataModel userData,
            Action<string> showMessage, Frame naviContent)
        {
            this.UpdateUserData = updateUserData;
            this.UserData = userData;
            this.ShowMessage = showMessage;
            this.NaviContent = naviContent;
        }
    }
}
