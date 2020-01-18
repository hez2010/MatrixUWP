#nullable enable
﻿using MatrixUWP.Extensions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Windows.Web.Http;
using Windows.Storage.Streams;

namespace MatrixUWP.Models.User
{
    public class UserModel
    {
        public static UserDataModel? CurrentUser { get; set; }
        public static async ValueTask<ResponseModel<UserDataModel>> SignInAsync(string userName, string password, string captcha = "")
        {
            var result = await (string.IsNullOrEmpty(captcha) ?
                App.MatrixHttpClient.PostJsonAsync("/api/users/login", new { username = userName, password })
                : App.MatrixHttpClient.PostJsonAsync("/api/users/login", new { username = userName, password, captcha }))
            .JsonAsync<ResponseModel<UserDataModel>>();
            if (result.Data?.SignedIn ?? false)
            {
                App.AppConfiguration.SavedUserName = userName;
                App.AppConfiguration.SavedPassword = password;
                CurrentUser = result.Data;
            }
            else
            {
                App.AppConfiguration.SavedUserName = "";
                App.AppConfiguration.SavedPassword = "";
            }
            return result;
        }

        public static ValueTask<ResponseModel<CaptchaDataModel>> FetchCaptchaAsync() => App.MatrixHttpClient.GetAsync(
                "/api/captcha"
            ).JsonAsync<ResponseModel<CaptchaDataModel>>();

        public static ValueTask<ResponseModel> SignOutAsync()
        {
            App.AppConfiguration.SavedUserName = "";
            App.AppConfiguration.SavedPassword = "";
            CurrentUser = null;
            return App.MatrixHttpClient.PostJsonAsync("/api/users/logout", new { }).JsonAsync<ResponseModel>();
        }

        public static async ValueTask<ResponseModel> UpdateProfileAsync(ProfileUpdateModel model)
        {
            return await App.MatrixHttpClient.PostJsonAsync("/api/users/profile", model)
                .JsonAsync<ResponseModel>();
        }

        public static async ValueTask<ResponseModel> UpdateAvatarAsync(IInputStream stream)
        {
            using var content = new HttpStreamContent(stream);
            content.Headers.ContentType = new Windows.Web.Http.Headers.HttpMediaTypeHeaderValue("image/jpeg");
            var data = new Dictionary<string, IHttpContent>
            {
                ["avatar"] = content
            };
            return await App.MatrixHttpClient.PostMultiPartAsync("/api/users/profile", data)
                .JsonAsync<ResponseModel>();
        }
    }
}
