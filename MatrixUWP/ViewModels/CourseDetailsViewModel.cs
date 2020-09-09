#nullable enable
using MatrixUWP.Models.Course;
using MatrixUWP.Models.User;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixUWP.ViewModels
{
    internal class CourseDetailsViewModel : INotifyPropertyChanged
    {
        private bool loading;
        private CourseInfoModel? course;
        private List<UserEssentialDataModel>? members;

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

        public List<UserEssentialDataModel>? Members
        {
            get => members;
            set
            {
                members = value;
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

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
