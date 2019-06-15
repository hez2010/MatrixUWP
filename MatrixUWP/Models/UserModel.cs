using MatrixUWP.Extensions;
using MatrixUWP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixUWP.Models
{
    class SignInDataModel
    {

    }
    class UserDataModel
    {

    }
    class UserModel
    {
        public static Task<SignInDataModel> SignInAsync(string userName, string password, string captcha = "")
            => App.MatrixHttpClient.PostAsync(
                new Uri(MatrixJsonHttpRequest.BaseUri, "/api/users/login"),
                new { username = userName, password = password })
            .JsonAsync<SignInDataModel>();
    }
}
