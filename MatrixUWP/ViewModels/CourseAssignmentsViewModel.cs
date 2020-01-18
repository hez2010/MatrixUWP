#nullable enable
﻿using MatrixUWP.Models.Course;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixUWP.ViewModels
{
    public class CourseAssignmentsViewModel : INotifyPropertyChanged
    {
        private bool loading;
        private List<CourseAssignmentInfoModel> assignments = new List<CourseAssignmentInfoModel>();

        public List<CourseAssignmentInfoModel> Assignments
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
