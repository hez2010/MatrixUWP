#nullable enable
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixUWP.Models.Course.Assignment.Choice
{
    public class ChoiceAssignmentElement : INotifyPropertyChanged
    {
        private bool isChecked;

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("$$hashKey")]
        public string HashKey { get; set; } = "";
        [JsonProperty("textStatus")]
        public int TextStatus { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; } = "";

        public bool IsChecked
        {
            get => isChecked;
            set
            {
                isChecked = value;
                OnPropertyChanged();
            }
        }
        public ChoiceAssignmentQuestion? Question { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}