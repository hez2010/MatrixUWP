#nullable enable
using MatrixUWP.Models.Course.Assignment;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixUWP.ViewModels
{
    public class CourseAssignmentsViewModel : INotifyPropertyChanged
    {
        private bool loading;
        private List<CourseAssignmentDetailsModel>? assignments;
        public string? Title { get; set; }
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
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
