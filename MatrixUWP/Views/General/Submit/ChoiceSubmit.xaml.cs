#nullable enable
using MatrixUWP.ViewModels;
using MatrixUWP.Views.Parameters.Submit;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatrixUWP.Views.General.Submit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChoiceSubmit : Page
    {
        private readonly ChoiceSubmitViewModel viewModel = new ChoiceSubmitViewModel();
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is ChoiceSubmitParameters p)
            {
                viewModel.Questions = p.Questions;
                viewModel.Description = p.Description;
                viewModel.Title = p.Title;
            }
        }
        public ChoiceSubmit()
        {
            this.InitializeComponent();
        }

        private async void MarkdownTextBlock_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(e.Link));
        }

        private async void MarkdownTextBlock_ImageClicked(object sender, LinkClickedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(e.Link));
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button btn) || !(btn.Tag is Action reset)) return;
            reset();
        }
    }
}
