#nullable enable
using MatrixUWP.Models.Course.Assignment.Choice;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixUWP.ViewModels
{
    internal class ChoiceSubmitViewModel : INotifyPropertyChanged
    {
        private bool loading;

        public bool Loading
        {
            get => loading;
            set
            {
                loading = value;
                OnPropertyChanged();
            }
        }
        public List<ChoiceAssignmentQuestion>? Questions { get; set; }
        public string Description { get; set; } = "";
        public string Title { get; set; } = "";

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
