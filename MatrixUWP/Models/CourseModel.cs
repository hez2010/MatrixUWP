using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MatrixUWP.Annotations;
using MatrixUWP.Extensions;
using Windows.UI;

namespace MatrixUWP.Models
{
    public class CourseInfoModel : INotifyPropertyChanged
    {
        private int courseId;
        private string status = "";
        private string type = "";
        private string courseName = "";
        private string schoolYear = "";
        private string description = "";
        private string semester = "";
        private string term = "";
        private int accessibleWhenClose;
        private int ipBinding;
        private string role = "";
        private int studentNum;
        private int progressingNum;
        private int unfinishedNum;
        private string teacher = "";
        private UserEssentialDataModel creator = new UserEssentialDataModel();

        [JsonProperty("course_id")]
        public int CourseId
        {
            get => courseId;
            set
            {
                courseId = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("status")]
        public string Status
        {
            get => status switch
            {
                "open" => "进行中",
                "close" => "已结束",
                _ => "未知"
            };

            set
            {
                status = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public Color StatusColor => status switch
        {
            "open" => Colors.CadetBlue,
            "close" => Colors.Orange,
            _ => Colors.Gray
        };

        [JsonProperty("type")]
        public string Type
        {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("course_name")]
        public string CourseName
        {
            get => courseName;
            set
            {
                courseName = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("school_year")]
        public string SchoolYear
        {
            get => schoolYear;
            set
            {
                schoolYear = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("description")]
        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("semester")]
        public string Semester
        {
            get => semester;
            set
            {
                semester = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("term")]
        public string Term
        {
            get => term;
            set
            {
                term = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("accessible_when_close")]
        public int AccessibleWhenClose
        {
            get => accessibleWhenClose;
            set
            {
                accessibleWhenClose = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("ip_binding")]
        public int IpBinding
        {
            get => ipBinding;
            set
            {
                ipBinding = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("role")]
        public string Role
        {
            get => role;
            set
            {
                role = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("student_num")]
        public int StudentNum
        {
            get => studentNum;
            set
            {
                studentNum = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("progressing_num")]
        public int ProgressingNum
        {
            get => progressingNum;
            set
            {
                progressingNum = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("unfinished_num")]
        public int UnfinishedNum
        {
            get => unfinishedNum;
            set
            {
                unfinishedNum = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("teacher")]
        public string Teacher
        {
            get => teacher;
            set
            {
                teacher = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("creator")]
        public UserEssentialDataModel Creator
        {
            get => creator;
            set
            {
                creator = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
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
