#nullable enable
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixUWP.Models.Course.Assignment.Choice
{
    class ChoiceAssignmentConfig : INotifyPropertyChanged
    {
        private List<ChoiceAssignmentQuestion>? questions;

        [JsonProperty("questions")]
        public List<ChoiceAssignmentQuestion>? Questions
        {
            get => questions;
            set
            {
                questions = value;
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
