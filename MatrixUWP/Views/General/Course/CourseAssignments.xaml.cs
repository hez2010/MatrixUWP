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
using MatrixUWP.Utils;
using MatrixUWP.ViewModels;
using MatrixUWP.Views.General.Submit;
using MatrixUWP.Views.Parameters.Course;
using MatrixUWP.Views.Parameters.Submit;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
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
        public static int LastCourseId = -1;

        public CourseAssignments()
        {
            NavigationCacheMode = NavigationCacheMode.Required;
            InitializeComponent();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            LastCourseId = parameters?.CourseId ?? -1;

            if (e.SourcePageType == typeof(CourseDetails))
            {
                var animation = ConnectedAnimationService.GetForCurrentView();
                animation.PrepareToAnimate("CourseTitleViewer", TitleViewer);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            parameters = e.Parameter as CourseAssignmentsParameters;
            if (parameters != null)
                viewModel.Title = parameters.Title;

            // Conntected Animations
            var animation = ConnectedAnimationService.GetForCurrentView();

            var descriptionAnimation = animation.GetAnimation("DescriptionViewer");
            if (descriptionAnimation != null)
            {
                var description = AssignmentView.FindChildOfName<ScrollViewer>("DescriptionViewer");
                if (description != null) descriptionAnimation.TryStart(description);
            }

            var titleAnimation = animation.GetAnimation("TitleViewer");
            if (titleAnimation != null)
            {
                var title = AssignmentView.FindChildOfName<TextBlock>("TitleViewer");
                if (title != null) titleAnimation.TryStart(title);
            }

            animation.GetAnimation("CourseTitleViewer")?.TryStart(TitleViewer);
        }

        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (parameters is null)
            {
                viewModel.Assignments = null;
                return;
            }
            if (LastCourseId == parameters.CourseId) return;
            viewModel.Assignments = null;
            viewModel.Loading = true;
            await Dispatcher.YieldAsync();

            try
            {
                var response = await CourseAssignmentModel.FetchCourseAssignmentListAsync(parameters?.CourseId ?? 0);
                if (response.Status != StatusCode.OK)
                {
                    AppModel.ShowMessage?.Invoke(response.Message);
                    return;
                }
                viewModel.Assignments = response.Data;
            }
            catch (Exception ex)
            {
                AppModel.ShowMessage?.Invoke(ex.Message);
#if DEBUG
                Debug.Fail(ex.Message, ex.StackTrace);
#endif
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
                    AppModel.ShowMessage?.Invoke(response.Message);
                    return;
                }
                response.Data.CopyTo(selectedItem);
                selectedItem.Loaded = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.Fail(ex.Message, ex.StackTrace);
#endif
                AppModel.ShowMessage?.Invoke(ex.Message);
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

        private void ShowSubmitPage(object config, CourseAssignmentDetailsModel model)
        {
            var animation = ConnectedAnimationService.GetForCurrentView();
            var description = AssignmentView.FindChildOfName<ScrollViewer>("DescriptionViewer");
            animation.PrepareToAnimate("DescriptionViewer", description);
            var title = AssignmentView.FindChildOfName<TextBlock>("TitleViewer");
            animation.PrepareToAnimate("TitleViewer", title);

            switch (config)
            {
                case ProgrammingAssignmentConfig asgnConfig:
                    {
                        void SetContent(string fileName, string content)
                        {
                            asgnConfig.SubmitContents[fileName] = content;
                        }

                        string GetContent(string fileName, bool isSupportFile)
                        {
                            return isSupportFile switch
                            {
                                true => (asgnConfig.SupportContents?.ContainsKey(fileName) ?? false) ? asgnConfig.SupportContents[fileName] : "",
                                false => asgnConfig.SubmitContents.ContainsKey(fileName) ? asgnConfig.SubmitContents[fileName] : ""
                            };
                        }

                        if (asgnConfig.Standard?.Support != null)
                        {
                            foreach (var i in asgnConfig.Standard.Support)
                            {
                                asgnConfig.SupportContents[i] = model.Files.FirstOrDefault(f => f.Name == i)?.Code ?? "";
                            }
                        }

                        AppModel.NavigateToPage?.Invoke(typeof(ProgrammingSubmit),
                            new ProgrammingSubmitParameters
                            {
                                Submissions = asgnConfig.Submission,
                                Supports = asgnConfig.Standard?.Support,
                                Languages = asgnConfig.Language,
                                Title = model.Title,
                                Description = model.Description,
                                CourseId = model.CourseId,
                                AssignmentId = model.CourseAssignmentId,
                                GetContent = GetContent,
                                SetContent = SetContent
                            },
                            new EntranceNavigationTransitionInfo());
                        break;
                    }
                case ChoiceAssignmentConfig asgnConfig:
                    AppModel.NavigateToPage?.Invoke(typeof(ChoiceSubmit),
                        new ChoiceSubmitParameters
                        {
                            Questions = asgnConfig.Questions,
                            Title = model.Title,
                            Description = model.Description,
                            CourseId = model.CourseId,
                            AssignmentId = model.CourseAssignmentId
                        },
                        new EntranceNavigationTransitionInfo());
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

                if (model.DeserialzedConfig is null)
                    model.DeserialzedConfig = model.Config.ToObject(model.ConfigType, jsonSerializer);
                if (model.DeserialzedConfig is null)
                {
                    AppModel.ShowMessage?.Invoke("题目配置错误");
                    return;
                }
                ShowSubmitPage(model.DeserialzedConfig, model);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.Fail(ex.Message, ex.StackTrace);
#endif
                AppModel.ShowMessage?.Invoke(ex.Message);
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
