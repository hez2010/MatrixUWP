#nullable enable
using MatrixUWP.Extensions;
using MatrixUWP.Models;
using MatrixUWP.Models.Course.Assignment;
using MatrixUWP.Models.Course.Assignment.Answer;
using MatrixUWP.Models.Course.Assignment.Choice;
using MatrixUWP.Models.Course.Assignment.File;
using MatrixUWP.Models.Course.Assignment.Output;
using MatrixUWP.Models.Course.Assignment.Programming;
using MatrixUWP.Models.Course.Assignment.Report;
using MatrixUWP.ViewModels;
using MatrixUWP.Views.Parameters.Course;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatrixUWP.Views.General.Course
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CourseAssignments : Page
    {
        private readonly CourseAssignmentsViewModel viewModel = new CourseAssignmentsViewModel();
        private CourseAssignmentsParameters? parameters;
        private static readonly JsonSerializer jsonSerializer = new JsonSerializer
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        public CourseAssignments()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            parameters = e.Parameter as CourseAssignmentsParameters;
        }

        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            viewModel.Loading = true;
            await Dispatcher.YieldAsync();

            try
            {
                var response = await CourseAssignmentModel.FetchCourseAssignmentListAsync(parameters?.CourseId ?? 0);
                if (response.Status != StatusCode.OK)
                {
                    parameters?.ShowMessage(response.Message);
                    return;
                }
                viewModel.Assignments = response.Data;
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

        private async void AssignmentView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0 || parameters is null) return;
            if (!(e.AddedItems.First() is CourseAssignmentDetailsModel selectedItem)) return;
            if (selectedItem.Loaded) return;
            selectedItem.Loading = true;
            await Dispatcher.YieldAsync();
            try
            {
                var response =
                    await CourseAssignmentModel.FetchCourseAssignmentAsync(
                        parameters.CourseId,
                        selectedItem.CourseAssignmentId);
                if (response.Status != StatusCode.OK)
                {
                    parameters.ShowMessage(response.Message);
                    return;
                }
                response.Data.CopyTo(selectedItem);
                selectedItem.Loaded = true;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message, ex.StackTrace);
                parameters.ShowMessage(ex.Message);
            }
            finally
            {
                selectedItem.Loading = false;
            }
        }
        private async void MarkdownTextBlock_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(e.Link));
        }

        private async void MarkdownTextBlock_ImageClicked(object sender, LinkClickedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(e.Link));
        }

        private void ShowSubmitPage(object config)
        {
            switch (config)
            {
                case ProgrammingAssignmentConfig asgnConfig:
                    Debug.WriteLine(asgnConfig.SerializeJson());
                    break;
                case ChoiceAssignmentConfig asgnConfig:
                    Debug.WriteLine(asgnConfig.SerializeJson());
                    break;
                case ReportAssignmentConfig asgnConfig:
                    Debug.WriteLine(asgnConfig.SerializeJson());
                    break;
                case FileAssignmentConfig asgnConfig:
                    Debug.WriteLine(asgnConfig.SerializeJson());
                    break;
                case OutputAssignmentConfig asgnConfig:
                    Debug.WriteLine(asgnConfig.SerializeJson());
                    break;
                case AnswerAssignmentConfig asgnConfig:
                    Debug.WriteLine(asgnConfig.SerializeJson());
                    break;
            }
        }

        private void Submit_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (!(AssignmentView.SelectedItem is CourseAssignmentDetailsModel model)) return;
            if (model.Config is null) return;
            try
            {
                if (model.ConfigType is null)
                {
                    model.ConfigType = model.ProblemTypeId switch
                    {
                        0 => typeof(ProgrammingAssignmentConfig),
                        1 => typeof(ChoiceAssignmentConfig),
                        2 => typeof(ReportAssignmentConfig),
                        3 => typeof(FileAssignmentConfig),
                        4 => typeof(OutputAssignmentConfig),
                        5 => throw new NotSupportedException("Problem blank fill problem is no longer supported."),
                        6 => typeof(AnswerAssignmentConfig),
                        _ => throw new NotSupportedException($"Problem type ${model.Type}(${model.ProblemTypeId}) is not supported.")
                    };
                }

                var obj = model.Config.ToObject(model.ConfigType, jsonSerializer);
                if (obj is null)
                {
                    parameters?.ShowMessage("题目配置错误");
                    return;
                }
                ShowSubmitPage(obj);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message, ex.StackTrace);
                parameters?.ShowMessage(ex.Message);
            }
        }

        private void Report_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void Star_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}
