#nullable enable
﻿using MatrixUWP.Extensions;
using MatrixUWP.Models;
using MatrixUWP.Models.Course;
using MatrixUWP.ViewModels;
using MatrixUWP.Views.Parameters.Course;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatrixUWP.Views.General.Course
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CourseDetails : Page
    {
        private readonly CourseDetailsViewModel viewModel = new CourseDetailsViewModel();
        private CourseDetailsParameters? parameters;
        public CourseDetails()
        {
            InitializeComponent();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            if (e.SourcePageType == typeof(CourseAssignments))
            {
                var animation = ConnectedAnimationService.GetForCurrentView();
                animation.PrepareToAnimate("CourseTitleViewer", TitleViewer);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            parameters = e.Parameter as CourseDetailsParameters;

            var animation = ConnectedAnimationService.GetForCurrentView();
            animation.GetAnimation("CourseTitleViewer")?.TryStart(TitleViewer);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AdjustElementsSize();
            viewModel.Loading = true;
            await Dispatcher.YieldAsync();
            try
            {
                var response = await CourseModel.FetchCourseAsync(parameters?.CourseId ?? 0);
                if (response.Status != StatusCode.OK)
                {
                    parameters?.ShowMessage(response.Message);
                    return;
                }
                viewModel.Course = response.Data;
            }
            catch (Exception ex)
            {
                parameters?.ShowMessage(ex.Message);
                Debug.Fail(ex.Message, ex.StackTrace);
            }
            finally
            {
                viewModel.Loading = false;
            }
        }

        private void AdjustElementsSize()
        {
            BorderLine.X2 = ActualWidth;
            DescriptionViewer.MaxHeight = Math.Max(ActualHeight - 150, 0);
            DescriptionViewer.MaxWidth = Math.Max(Container.ActualWidth - 16, 0);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e) => AdjustElementsSize();
        
        private async void MarkdownTextBlock_LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(e.Link));
        }

        private async void MarkdownTextBlock_ImageClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(e.Link));
        }

        private void ViewAssignment_Click(object sender, RoutedEventArgs e)
        {
            parameters?.NavigateToPage(typeof(CourseAssignments),
                typeof(CourseAssignmentsParameters),
                new { parameters.CourseId, Title = viewModel.Course?.CourseName },
                new EntranceNavigationTransitionInfo());
        }
    }
}
