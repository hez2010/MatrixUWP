using MatrixUWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixUWP.Views.Parameters
{
    class CommonParameters
    {
        public Action<UserDataModel>? UpdateUserData { get; set; }
        public UserDataModel UserData { get; set; } = new UserDataModel();
        public Action<string>? ShowMessage { get; set; }
    }
}
