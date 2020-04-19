#nullable enable
using MatrixUWP.Models.User;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixUWP.Models.Course.Assignment
{
    public class CourseAssignmentDetailsModel : INotifyPropertyChanged
    {
        private bool loading;
        private string description = "";

        [JsonProperty("type")]
        public string Type { get; set; } = "";
        public string TypeDisplayText => ProblemTypeId switch
        {
            0 => "编程题",
            1 => "选择题",
            2 => "报告题",
            3 => "文件上传题",
            4 => "程序输出题",
            5 => "程序填空题",
            6 => "简答题",
            _ => Type
        };

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

        public string GradeAtEndDescription => GradeAtEnd == 0 ? "实时评测" : "定时评测";

        [JsonProperty("pub_answer")]
        public int PubAnswer { get; set; }

        [JsonProperty("lib_id")]
        public int LibraryId { get; set; }

        [JsonProperty("prob_id")]
        public int ProblemId { get; set; }
        /// <summary>
        /// <para>Problem Type:</para>
        /// <list type="table">
        /// <item>0 - Programming problem</item>
        /// <item>1 - Choice problem</item>
        /// <item>2 - Report</item>
        /// <item>3 - Fileupload problem</item>
        /// <item>4 - Program output problem</item>
        /// <item>5 - Problem blank fill problem (<strong>NOT SUPPORTED</strong>)</item>
        /// <item>6 - Answer problem</item>
        /// </list>
        /// </summary>

        [JsonProperty("ptype_id")]
        public int ProblemTypeId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; } = "";

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
        public int PlagiarismCheck { get; set; }

        [JsonProperty("submit_limitation")]
        public int SubmitLimit { get; set; }

        [JsonProperty("standard_score")]
        public int StandardScore { get; set; }

        [JsonProperty("is_started")]
        public bool IsStarted { get; set; }

        [JsonProperty("files")]
        public List<AssignmentFile>? Files { get; set; }

        [JsonProperty("author")]
        public UserEssentialDataModel Author { get; set; } = new UserEssentialDataModel();

        [JsonProperty("config")]
        public JObject? Config { get; set; }
        public object? DeserialzedConfig { get; set; }

        public bool Loading
        {
            get => loading;
            set
            {
                loading = value;
                OnPropertyChanged();
            }
        }

        public Type? ConfigType { get; set; }

        public bool Loaded { get; set; } = false;


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
