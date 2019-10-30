using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MatrixUWP.Extensions;
using Windows.UI;

namespace MatrixUWP.Models
{
    public class CourseInfoModel : INotifyPropertyChanged
    {
        private string status = "";
        private string role = "";

        [JsonProperty("course_id")]
        public int CourseId { get; set; }

        [JsonProperty("status")]
        public string Status
        {
            get => status.ToLowerInvariant() switch
            {
                "open" => "进行中",
                "close" => "已结束",
                _ => "未知"
            };

            set
            {
                status = value;
            }
        }

        [JsonIgnore]
        public Color StatusColor => status.ToLowerInvariant() switch
        {
            "open" => Colors.CadetBlue,
            "close" => Colors.Orange,
            _ => Colors.Gray
        };

        [JsonProperty("type")]
        public string Type { get; set; } = "";

        [JsonProperty("course_name")]
        public string CourseName { get; set; } = "";

        [JsonProperty("school_year")]
        public string SchoolYear { get; set; } = "";

        [JsonProperty("description")]
        public string Description { get; set; } = "";

        [JsonProperty("semester")]
        public string Semester { get; set; } = "";

        [JsonProperty("term")]
        public string Term { get; set; } = "";

        [JsonProperty("accessible_when_close")]
        public int AccessibleWhenClose { get; set; }

        [JsonProperty("ip_binding")]
        public int IpBinding { get; set; }

        [JsonProperty("role")]
        public string Role
        {
            get => role.ToLowerInvariant() switch
            {
                "teacher" => "教师",
                "ta" => "助教",
                "student" => "学生",
                "admin" => "管理员",
                _ => "未知"
            };
            set
            {
                role = value;
            }
        }

        [JsonProperty("student_num")]
        public int StudentNum { get; set; }

        [JsonProperty("progressing_num")]
        public int ProgressingNum { get; set; }

        [JsonProperty("unfinished_num")]
        public int UnfinishedNum { get; set; }

        [JsonProperty("teacher")]
        public string Teacher { get; set; } = "";

        [JsonProperty("creator")]
        public UserEssentialDataModel Creator { get; set; } = new UserEssentialDataModel();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class CourseModel
    {
        public static async ValueTask<ResponseModel<List<CourseInfoModel>>> FetchCourseListAsync() =>
            await App.MatrixHttpClient.GetAsync("/api/courses")
                .JsonAsync<ResponseModel<List<CourseInfoModel>>>();

        public static async ValueTask<ResponseModel<CourseInfoModel>> FetchCourseAsync(int courseId) =>
            await App.MatrixHttpClient.GetAsync($"/api/courses/{courseId}")
                .JsonAsync<ResponseModel<CourseInfoModel>>();
    }
}
