using MatrixUWP.Extensions;
using MatrixUWP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MatrixUWP.Models
{
    class UserDataModel
    {
        [JsonPropertyName("captcha")]
        public bool Captcha { get; set; }
        [JsonPropertyName("user_id")]
        public string UserId { get; set; } = "";
        [JsonPropertyName("nickname")]
        public string NickName { get; set; } = "";
        [JsonPropertyName("realname")]
        public string RealName { get; set; } = "";
        [JsonPropertyName("username")]
        public string UserName { get; set; } = "";
        [JsonPropertyName("is_valid")]
        public int IsValid { get; set; }
        [JsonPropertyName("homepage")]
        public string HomePage { get; set; } = "";
        [JsonPropertyName("phone")]
        public string Phone { get; set; } = "";
        [JsonPropertyName("email")]
        public string Eamil { get; set; } = "";
        [JsonPropertyName("user_addition")]
        public dynamic? UserAddition { get; set; }
        [JsonPropertyName("student_id")]
        public string StudentId { get; set; } = "";
        [JsonPropertyName("academy")]
        public string Academy { get; set; } = "";
        [JsonPropertyName("specialty")]
        public string Specialty { get; set; } = "";
        [JsonPropertyName("isInLib")]
        public bool IsInLib { get; set; }
        [JsonPropertyName("canCreateLib")]
        public bool CanCreateLib { get; set; }
    }

    class UserModel
    {
        public static Task<ResponseModel<UserDataModel>> SignInAsync(string userName, string password, string captcha = "")
            => App.MatrixHttpClient.PostAsync(
                new Uri(MatrixJsonHttpRequest.BaseUri, "/api/users/login"),
                new { username = userName, password = password })
            .JsonAsync<ResponseModel<UserDataModel>>();
    }
}
