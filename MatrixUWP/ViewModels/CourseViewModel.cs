﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MatrixUWP.Annotations;
using MatrixUWP.Models;

namespace MatrixUWP.ViewModels
{
    class CourseViewModel : INotifyPropertyChanged
    {
        private bool loading;
        private List<CourseInfoModel> courses = new List<CourseInfoModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        public List<CourseInfoModel> Courses
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}