#nullable enable
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixUWP.ViewModels
{
    internal class ProgrammingSubmitViewModel : INotifyPropertyChanged
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

        public string Description { get; set; } = "";
        public string Title { get; set; } = "";
        public List<ProgrammingFileModel>? Files { get; set; }
        public int RemainingSubmitTimes { get; set; }
        public string RemainingSubmitText => $"剩余提交次数：{(RemainingSubmitTimes == -1 ? "无限" : RemainingSubmitTimes.ToString())}";

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
