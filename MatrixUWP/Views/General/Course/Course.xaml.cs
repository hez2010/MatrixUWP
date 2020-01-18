#nullable enable
﻿using System;
using MatrixUWP.Extensions;
using MatrixUWP.Models;
using MatrixUWP.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MatrixUWP.Views.Parameters.Course;
using System.Linq;
using System.Diagnostics;
using MatrixUWP.Models.Course;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatrixUWP.Views.General.Course
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Course : Page
    {
        private readonly CourseViewModel viewModel = new CourseViewModel();
        private CourseParameters? parameters;

        public Course()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            parameters = e.Parameter as CourseParameters;
        }

        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            viewModel.Loading = true;
            await Dispatcher.YieldAsync();

            try
            {
                var coursesResponse = await CourseModel.FetchCourseListAsync();
                if (coursesResponse.Status == StatusCode.OK) viewModel.Courses = coursesResponse.Data;
                else parameters?.ShowMessage(coursesResponse.Message);
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

        private void CoursesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.AddedItems.First() is CourseInfoModel course)) return;
            parameters?.NavigateToPage(typeof(CourseDetails), typeof(CourseDetailsParameters), new { course.CourseId });
        }
    }
}
