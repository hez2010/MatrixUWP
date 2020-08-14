#nullable enable
using MatrixUWP.Models.Course.Assignment;
using MatrixUWP.Models.Submission.Course;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixUWP.ViewModels
{
    public class CourseAssignmentsViewModel : INotifyPropertyChanged
    {
        private bool loading;
        private List<CourseAssignmentDetailsModel>? assignments;
        private string? title;
        private bool loadingRank;
        private List<RankModel>? rankInfo;
        private List<CourseSubmissionInfoModel>? submissionInfo;
        private bool loadingSubmission;

        public bool LoadingRank
        {
            get => loadingRank;
            set
            {
                loadingRank = value;
                OnPropertyChanged();
            }
        }

        public List<RankModel>? RankInfo
        {
            get => rankInfo;
            set
            {
                rankInfo = value;
                OnPropertyChanged();
            }
        }

        public bool LoadingSubmission
        {
            get => loadingSubmission;
            set
            {
                loadingSubmission = value;
                OnPropertyChanged();
            }
        }

        public List<CourseSubmissionInfoModel>? SubmissionInfo
        {
            get => submissionInfo;
            set
            {
                submissionInfo = value;
                OnPropertyChanged();
            }
        }

        public string? Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }
        public List<CourseAssignmentDetailsModel>? Assignments
        {
            get => assignments;
            set
            {
                assignments = value;
                OnPropertyChanged();
            }
        }

        public bool Loading
        {
            get => loading;
            set
            {
                loading = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
