#nullable enable
using MatrixUWP.Models.Message;
using MatrixUWP.Shared.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixUWP.ViewModels
{
    public class MessageViewModel : INotifyPropertyChanged
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

        public ObservableDictionary<MessageSender, ObservableCollection<MessageContent>> Messages { get; } = new ObservableDictionary<MessageSender, ObservableCollection<MessageContent>>();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
