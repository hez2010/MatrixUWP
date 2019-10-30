using MatrixUWP.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MatrixUWP.Models
{
    public class AssignmentLimits
    {
        [JsonProperty("time")]
        public long Time { get; set; }
        [JsonProperty("memory")]
        public long Memory { get; set; }
    }
    public class AssignmentGrading
    {
        [JsonProperty("google tests")]
        public int GoogleTests { get; set; }
        [JsonProperty("memory check")]
        public int MemoryCheck { get; set; }
        [JsonProperty("random check")]
        public int RandomCheck { get; set; }
        [JsonProperty("static check")]
        public int StaticCheck { get; set; }
        [JsonProperty("compile check")]
        public int CompileCheck { get; set; }
        [JsonProperty("standard check")]
        public int StandardCheck { get; set; }
    }

    public class AssignmentConfig
    {
        [JsonProperty("limits")]
        public AssignmentLimits Limits { get; set; } = new AssignmentLimits();

        [JsonProperty("grading")]
        public AssignmentGrading Grading { get; set; } = new AssignmentGrading();

        [JsonProperty("language")]
        public List<string> Language { get; set; } = new List<string>();

        // TODO： other properties

        [JsonProperty("standard_score")]
        public int StandardScore { get; set; }

        [JsonProperty("standard_language")]
        public string StandardLanguage { get; set; }
    }
    public class CourseAssignmentInfoModel : INotifyPropertyChanged
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("course_id")]
        public int CourseId { get; set; }

        [JsonProperty("ca_id")]
        public int CourseAssignmentId { get; set; }

        [JsonProperty("startdate")]
        public DateTime StartTime { get; set; }

        [JsonProperty("enddate")]
        public DateTime EndTime { get; set; }

        [JsonProperty("grade_at_end")]
        public int GradeAtEnd { get; set; }

        [JsonProperty("pub_answer")]
        public int PubAnswer { get; set; }

        [JsonProperty("lib_id")]
        public int LibraryId { get; set; }

        [JsonProperty("prob_id")]
        public int ProblemId { get; set; }

        [JsonProperty("ptype_id")]
        public int ProblemTypeId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; } = "";

        [JsonProperty("description")]
        public string Description { get; set; } = "";

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("total_student")]
        public int TotalStudent { get; set; }

        [JsonProperty("submit_times")]
        public int SubmitTimes { get; set; }

        [JsonProperty("submit_student_num")]
        public int SubmitStudentNumber { get; set; }

        [JsonProperty("asgn_id")]
        public int AssignmentId { get; set; }

        [JsonProperty("plcheck")]
        public int PlCheck { get; set; }

        [JsonProperty("submit_limitation")]
        public int SubmitLimit { get; set; }

        [JsonProperty("standard_score")]
        public int StandardScore { get; set; }

        [JsonProperty("is_started")]
        public bool IsStarted { get; set; }

        [JsonProperty("files")]
        public List<string> Files { get; set; } = new List<string>();

        [JsonProperty("author")]
        public UserEssentialDataModel Author { get; set; } = new UserEssentialDataModel();


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class CourseAssignmentModel
    {
        public static async ValueTask<ResponseModel<List<CourseAssignmentInfoModel>>> FetchCourseAssignmentListAsync(int courseId) =>
              await App.MatrixHttpClient.GetAsync($"/api/courses/{courseId}/assignments")
                  .JsonAsync<ResponseModel<List<CourseAssignmentInfoModel>>>();

        public static async ValueTask<ResponseModel<CourseAssignmentInfoModel>> FetchCourseAsync(int courseId, int courseAssignmentId) =>
            await App.MatrixHttpClient.GetAsync($"/api/courses/{courseId}/assignments/{courseAssignmentId}")
                .JsonAsync<ResponseModel<CourseAssignmentInfoModel>>();
    }
}
