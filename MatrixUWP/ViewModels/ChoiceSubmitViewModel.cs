#nullable enable
using MatrixUWP.Models.Course.Assignment.Choice;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixUWP.ViewModels
{
    class ChoiceSubmitViewModel : INotifyPropertyChanged
    {
        public List<ChoiceAssignmentQuestion> Questions { get; set; } = new List<ChoiceAssignmentQuestion>();
        public string Description { get; set; } = "";
        public string Title { get; set; } = "";

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
