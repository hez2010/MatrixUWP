﻿#nullable enable
using MatrixUWP.Models;
using MatrixUWP.Models.Course;
using MatrixUWP.Parameters.Course;
using MatrixUWP.Shared.Models;
using MatrixUWP.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using Windows.Media.ContentRestrictions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatrixUWP.Views.Course
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

            try
            {
                var cres = await CourseModel.FetchCourseAsync(parameters?.CourseId ?? 0);
                if (cres?.Status != StatusCode.OK)
                {
                    AppModel.ShowMessage?.Invoke(cres?.Message ?? "课程获取失败");
                    return;
                }
                viewModel.Course = cres.Data;

                var mres = await CourseModel.FetchCourseMembersAsync(parameters?.CourseId ?? 0);
                if (mres?.Status != StatusCode.OK)
                {
                    AppModel.ShowMessage?.Invoke(cres?.Message ?? "课程成员获取失败");
                    return;
                }
                mres.Data.Sort((i, j) => i.Role == j.Role ? 0 : (i.Role, j.Role) switch
                {
                    ("teacher", _) => -1,
                    ("TA", "teacher") => 1,
                    ("TA", _) => -1,
                    _ => 1
                });
                viewModel.Members = mres.Data;
            }
            catch (Exception ex)
            {
                AppModel.ShowMessage?.Invoke(ex.Message);
#if FAIL_ON_DEBUG
                Debug.Fail(ex.Message, ex.StackTrace);
#endif
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

        private async void MarkdownTextBlock_LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e) => await Windows.System.Launcher.LaunchUriAsync(new Uri(e.Link));

        private async void MarkdownTextBlock_ImageClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e) => await Windows.System.Launcher.LaunchUriAsync(new Uri(e.Link));

        private void ViewAssignment_Click(object sender, RoutedEventArgs e)
        {
            if (parameters is null) return;
            AppModel.NavigateToPage?.Invoke(typeof(CourseAssignments),
                new CourseAssignmentsParameters { CourseId = parameters.CourseId, Title = viewModel.Course?.CourseName }, -1);
        }
    }
}
