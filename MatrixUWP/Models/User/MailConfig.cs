#nullable enable
﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace MatrixUWP.Models.User
{
    public class MailConfig : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private bool exam;
        private bool course;
        private bool library;
        private bool assignment;

        [JsonProperty("exam")]
        public bool Exam
        {
            get => exam;
            set
            {
                exam = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("course")]
        public bool Course
        {
            get => course;
            set
            {
                course = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("library")]
        public bool Library
        {
            get => library;
            set
            {
                library = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("course_assignment")]
        public bool Assignment
        {
            get => assignment;
            set
            {
                assignment = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
