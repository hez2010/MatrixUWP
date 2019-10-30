using MatrixUWP.Models;
using System;

namespace MatrixUWP.Views.Parameters
{
    class CommonParameters
    {
        public Action<UserDataModel> UpdateUserData { get; set; }
        public UserDataModel UserData { get; set; }
        public Action<string> ShowMessage { get; set; }
        public Action<Type, Type, object> NavigateToPage { get; set; }

        public CommonParameters(CommonParameters param)
        {
            this.UpdateUserData = param.UpdateUserData;
            this.UserData = param.UserData;
            this.ShowMessage = param.ShowMessage;
            this.NavigateToPage = param.NavigateToPage;
        }

        public CommonParameters(Action<UserDataModel> updateUserData, UserDataModel userData,
            Action<string> showMessage, Action<Type, Type, object> navigateToPage)
        {
            this.UpdateUserData = updateUserData;
            this.UserData = userData;
            this.ShowMessage = showMessage;
            this.NavigateToPage = navigateToPage;
        }
    }
}
