#nullable enable
using Microsoft.UI.Xaml.Controls;
using Monaco.Editor;
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

        private StandaloneEditorConstructionOptions options = new StandaloneEditorConstructionOptions();

        public StandaloneEditorConstructionOptions Options
        {
            get => options;
            set
            {
                var oldOptions = options;
                options = value;
                OnPropertyChanged();
                if (oldOptions.ReadOnly != value.ReadOnly)
                {
                    OnPropertyChanged(nameof(ReadOnlyIcon));
                }
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
                    if (!(options.ReadOnly ?? false))
                        SetContent?.Invoke(FileName, value ?? "");
                }
                else
                {
                    SuppressSetDispatcher = false;
                }

                OnPropertyChanged();
            }
        }
        public bool IsSupportFile { get; set; }

        private static FontIconSource LockIcon = new FontIconSource
        {
            Glyph = "\uE72E",
            FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe MDL2 Assets")
        };

        private static FontIconSource UnlockIcon = new FontIconSource
        {
            Glyph = "\uE785",
            FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe MDL2 Assets")
        };

        public IconSource ReadOnlyIcon => (options.ReadOnly ?? false) ? LockIcon : UnlockIcon;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
