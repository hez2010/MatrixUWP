﻿#nullable enable
using MatrixUWP.Shared.Utils;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MatrixUWP.Models.User
{
    public class UserEssentialDataModel : INotifyPropertyChanged
    {
        private string realName = "";
        private string userName = "";
        private string phone = "";
        private string email = "";
        private string homePage = "";
        private string role = "";

        public event PropertyChangedEventHandler? PropertyChanged;

        public string RoleText => role switch
        {
            "teacher" => "教师",
            "TA" => "助教",
            "student" => "学生",
            _ => "未知"
        };

        [JsonProperty("role")]
        public string Role
        {
            get => role;
            set
            {
                role = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RoleText));
            }
        }

        [JsonProperty("realname")]
        public string RealName
        {
            get => realName;
            set
            {
                realName = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("username")]
        public string UserName
        {
            get => userName;
            set
            {
                userName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Avatar));
            }
        }

        [JsonProperty("phone")]
        public string Phone
        {
            get => phone;
            set
            {
                phone = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("email")]
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("homepage")]
        public string HomePage
        {
            get => homePage;
            set
            {
                homePage = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public ImageSource Avatar
        {
            get
            {
                var bitmap = new BitmapImage();
                if (string.IsNullOrEmpty(UserName))
                {
                    bitmap.UriSource = new Uri("ms-appx:///Assets/Home/user.png");
                }
                else
                {
                    try
                    {
                        bitmap.UriSource = new Uri(HttpUtils.MatrixHttpClient.BaseUri, $"/api/users/profile/avatar?username={UserName}&t={DateTime.Now.Ticks}");
                    }
                    catch
                    {
                        bitmap.UriSource = new Uri("ms-appx:///Assets/Home/user.png");
                    }
                }

                return bitmap;
            }
        }

        public virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
