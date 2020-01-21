#nullable enable
using MatrixUWP.Models.Course;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixUWP.ViewModels
{
    class CourseDetailsViewModel : INotifyPropertyChanged
    {
        private bool loading;
        private CourseInfoModel? course;

        public event PropertyChangedEventHandler? PropertyChanged;

        public CourseInfoModel? Course
        {
            get => course;
            set
            {
                course = value;
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
