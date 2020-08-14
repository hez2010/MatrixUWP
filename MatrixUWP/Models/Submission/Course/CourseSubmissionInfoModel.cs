#nullable enable
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixUWP.Models.Submission.Course
{
    public class CourseSubmissionInfoModel : INotifyPropertyChanged
    {
        private bool loadingReport;
        private List<SubmissionReportModel>? report;

        public bool LoadingReport
        {
            get => loadingReport;
            set
            {
                loadingReport = value;
                OnPropertyChanged();
            }
        }

        public List<SubmissionReportModel>? Report
        {
            get => report; 
            set
            {
                report = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("sub_ca_id")]
        public int SubmissionId { get; set; }
        [JsonProperty("sub_asgn_id")]
        public int AssignmentId { get; set; }
        [JsonProperty("submit_at")]
        public DateTime SubmitAt { get; set; }
        [JsonProperty("grade")]
        public int? Grade { get; set; }
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        public string SubmitAtText => SubmitAt.ToString("yyyy/MM/dd HH:mm:ss");
        public bool HasReport => Grade != null;

        public event PropertyChangedEventHandler? PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
