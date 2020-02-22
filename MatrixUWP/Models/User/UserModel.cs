#nullable enable
using MatrixUWP.Services;
using MatrixUWP.Shared.Extensions;
using MatrixUWP.Shared.Models;
using MatrixUWP.Shared.Utils;
using System.Threading.Tasks;
using Windows.Storage;

namespace MatrixUWP.Models.User
{
    public class UserModel
    {
        public static void UpdateUserData(UserDataModel userData)
        {
            userData.CopyTo(CurrentUser);
        }

        public static UserDataModel CurrentUser { get; } = new UserDataModel();
        public static async ValueTask<ResponseModel<UserDataModel>> SignInAsync(string userName, string password, string captcha = "")
        {
            var result = await (string.IsNullOrEmpty(captcha) ?
                HttpUtils.MatrixHttpClient.PostJsonAsync("/api/users/login", new { username = userName, password })
                : HttpUtils.MatrixHttpClient.PostJsonAsync("/api/users/login", new { username = userName, password, captcha }))
            .JsonAsync<ResponseModel<UserDataModel>>();
            if (result.Data?.SignedIn ?? false)
            {
                AppModel.AppConfiguration.SavedUserName = userName;
                AppModel.AppConfiguration.SavedPassword = password;
                result.Data.CopyTo(CurrentUser);
                await PushService.RegistTaskAsync();
            }
            else
            {
                AppModel.AppConfiguration.SavedUserName = "";
                AppModel.AppConfiguration.SavedPassword = "";
            }
            return result;
        }

        public static ValueTask<ResponseModel<UserDataModel>> GetUserProfile()
            => HttpUtils.MatrixHttpClient.GetAsync("/api/users/profile")
                .JsonAsync<ResponseModel<UserDataModel>>();

        public static ValueTask<ResponseModel<CaptchaDataModel>> FetchCaptchaAsync()
            => HttpUtils.MatrixHttpClient.GetAsync("/api/captcha")
                .JsonAsync<ResponseModel<CaptchaDataModel>>();

        public static async ValueTask<ResponseModel> SignOutAsync()
        {
            AppModel.AppConfiguration.SavedUserName = "";
            AppModel.AppConfiguration.SavedPassword = "";
            new UserDataModel().CopyTo(CurrentUser);
            await PushService.RegistTaskAsync();
            return await HttpUtils.MatrixHttpClient.PostJsonAsync("/api/users/logout", new { }).JsonAsync<ResponseModel>();
        }

        public static ValueTask<ResponseModel> UpdateProfileAsync(ProfileUpdateModel model)
            => HttpUtils.MatrixHttpClient.PostJsonAsync("/api/users/profile", model)
                .JsonAsync<ResponseModel>();

        public static ValueTask<ResponseModel> UpdateAvatarAsync(StorageFile file)
            => HttpUtils.MatrixHttpClient.PostFileAsync("/api/users/profile", "avatar", file)
                .JsonAsync<ResponseModel>();
    }
}
