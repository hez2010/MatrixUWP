#nullable enable
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixUWP.ViewModels
{
    public class ProgrammingFileModel : INotifyPropertyChanged
    {
        /// <summary>
        /// FileName, IsSupportFile, Result
        /// </summary>
        public Func<string, bool, string>? GetContent;
        /// <summary>
        /// FileName, Content
        /// </summary>
        public Action<string, string>? SetContent;
        public bool SuppressSetDispatcher = false;
        private string? language;

        public string? Language
        {
            get => language;
            set
            {
                language = value;
                OnPropertyChanged();
            }
        }

        private bool readOnly;

        public bool ReadOnly
        {
            get => readOnly;
            set
            {
                readOnly = value;
                OnPropertyChanged();
            }
        }
        public string FileName { get; set; } = "";
        public string? Content
        {
            get => GetContent?.Invoke(FileName, IsSupportFile);
            set
            {
                if (!SuppressSetDispatcher)
                {
                    if (!ReadOnly)
                        SetContent?.Invoke(FileName, value ?? "");
                }
                else SuppressSetDispatcher = false;
                OnPropertyChanged();
            }
        }
        public bool IsSupportFile { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
