#nullable enable
using System;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatrixUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class About : Page
    {
        private string Version { get; } = GetAppVersion();
        public static string ReleaseNotes = $"2020/08/26 {GetAppVersion()}\n1. 完善成绩报告解析\n2. 允许复制评测日志\n3. 添加评价和反馈入口";

        public About()
        {
            InitializeComponent();
        }
        public static string GetAppVersion()
        {
            var version = Package.Current.Id.Version;
            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        private async void Rate_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?ProductId=9NB87BN58323"));
        }

        private async void Feedback_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://matrix.sysu.edu.cn/feedback"));
        }
    }
}
