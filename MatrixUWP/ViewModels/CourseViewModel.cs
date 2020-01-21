#nullable enable
using MatrixUWP.Models.Course;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixUWP.ViewModels
{
    class CourseViewModel : INotifyPropertyChanged
    {
        private bool loading;
        private List<CourseInfoModel>? courses;

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<CourseInfoModel>? Courses
        {
            get => courses;
            set
            {
                courses = value;
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

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
