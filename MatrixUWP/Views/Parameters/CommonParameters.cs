using MatrixUWP.Models;
using System;

namespace MatrixUWP.Views.Parameters
{
    class CommonParameters
    {
        public Action<UserDataModel>? UpdateUserData { get; set; }
        public UserDataModel UserData { get; set; } = new UserDataModel();
        public Action<string>? ShowMessage { get; set; }
    }
}
