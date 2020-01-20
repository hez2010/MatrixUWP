#nullable enable
using Monaco.Editor;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixUWP.ViewModels
{
    public class ProgrammingFileModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Index, IsSupportFile, Result
        /// </summary>
        public Func<int, bool, string>? GetContent;
        public Action<int, string>? SetContent;
        public int Index { get; set; }
        public bool SuppressSetDispatcher;
        private IEditorConstructionOptions? editorOptions;

        public IEditorConstructionOptions? EditorOptions
        {
            get => editorOptions;
            set
            {
                editorOptions = value;
                OnPropertyChanged();
            }
        }
        public string FileName { get; set; } = "";
        public string? Content
        {
            get => GetContent?.Invoke(Index, !NeedsSubmit);
            set
            {
                if (!SuppressSetDispatcher)
                {
                    if (!(EditorOptions?.ReadOnly ?? false))
                        SetContent?.Invoke(Index, value ?? "");
                }
                else SuppressSetDispatcher = false;
                OnPropertyChanged();
            }
        }
        public bool NeedsSubmit { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
