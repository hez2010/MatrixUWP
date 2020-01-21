#nullable enable
using MatrixUWP.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace MatrixUWP.Models.User
{
    public class UserModel
    {
        public static void UpdateUserData(UserDataModel userData)
        {
            userData.CopyTo(CurrentUser);
        }

        public static UserDataModel CurrentUser { get; set; } = new UserDataModel();
        public static async ValueTask<ResponseModel<UserDataModel>> SignInAsync(string userName, string password, string captcha = "")
        {
            var result = await (string.IsNullOrEmpty(captcha) ?
                AppModel.MatrixHttpClient.PostJsonAsync("/api/users/login", new { username = userName, password })
                : AppModel.MatrixHttpClient.PostJsonAsync("/api/users/login", new { username = userName, password, captcha }))
            .JsonAsync<ResponseModel<UserDataModel>>();
            if (result.Data?.SignedIn ?? false)
            {
                AppModel.AppConfiguration.SavedUserName = userName;
                AppModel.AppConfiguration.SavedPassword = password;
                CurrentUser = result.Data;
            }
            else
            {
                AppModel.AppConfiguration.SavedUserName = "";
                AppModel.AppConfiguration.SavedPassword = "";
            }
            return result;
        }

        public static ValueTask<ResponseModel<CaptchaDataModel>> FetchCaptchaAsync()
        {
            return AppModel.MatrixHttpClient.GetAsync(
"/api/captcha"
).JsonAsync<ResponseModel<CaptchaDataModel>>();
        }

        public static ValueTask<ResponseModel> SignOutAsync()
        {
            AppModel.AppConfiguration.SavedUserName = "";
            AppModel.AppConfiguration.SavedPassword = "";
            new UserDataModel().CopyTo(CurrentUser);
            return AppModel.MatrixHttpClient.PostJsonAsync("/api/users/logout", new { }).JsonAsync<ResponseModel>();
        }

        public static async ValueTask<ResponseModel> UpdateProfileAsync(ProfileUpdateModel model)
        {
            return await AppModel.MatrixHttpClient.PostJsonAsync("/api/users/profile", model)
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
            return await AppModel.MatrixHttpClient.PostMultiPartAsync("/api/users/profile", data)
                .JsonAsync<ResponseModel>();
        }
    }
}
