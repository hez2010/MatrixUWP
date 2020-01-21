#nullable enable
using MatrixUWP.Converters;
using MatrixUWP.Extensions;
using MatrixUWP.Models;
using MatrixUWP.Models.Submission;
using MatrixUWP.Models.Submission.Programming;
using MatrixUWP.Utils;
using MatrixUWP.ViewModels;
using MatrixUWP.Views.General.Course;
using MatrixUWP.Views.Parameters.Submit;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Monaco;
using Monaco.Editor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatrixUWP.Views.General.Submit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProgrammingSubmit : Page
    {
        private readonly ProgrammingSubmitViewModel viewModel = new ProgrammingSubmitViewModel();
        private ProgrammingSubmitParameters? parameters;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var animation = ConnectedAnimationService.GetForCurrentView();
            animation.GetAnimation("DescriptionViewer")?.TryStart(DescriptionViewer);
            animation.GetAnimation("TitleViewer")?.TryStart(TitleViewer);

            if (e.Parameter is ProgrammingSubmitParameters p)
            {
                parameters = p;
                viewModel.Description = p.Description;
                viewModel.Title = p.Title;
                viewModel.Languages = p.Languages;
                viewModel.Files = new List<ProgrammingFileModel>();
                var languageConverter = new LanguageConverter();
                if (p.Submissions != null)
                {
                    foreach (var i in p.Submissions)
                    {
                        viewModel.Files.Add(new ProgrammingFileModel
                        {
                            SuppressSetDispatcher = true,
                            FileName = i,
                            EditorOptions = new IEditorConstructionOptions
                            {
                                ReadOnly = true,
                                Language = languageConverter.Convert(p.Languages?.FirstOrDefault()!, typeof(string), null!, null!)?.ToString() ?? ""
                            },
                            IsSupportFile = false,
                            SetContent = p.SetContent,
                            GetContent = p.GetContent
                        });
                    }
                }
                if (p.Supports != null)
                {
                    foreach (var i in p.Supports)
                    {
                        viewModel.Files.Add(new ProgrammingFileModel
                        {
                            SuppressSetDispatcher = true,
                            FileName = i,
                            EditorOptions = new IEditorConstructionOptions
                            {
                                ReadOnly = true,
                                Language = languageConverter.Convert(p.Languages?.FirstOrDefault()!, typeof(string), null!, null!)?.ToString() ?? ""
                            },
                            IsSupportFile = true,
                            GetContent = p.GetContent
                        });
                    }
                }
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.SourcePageType == typeof(CourseAssignments))
            {
                var animation = ConnectedAnimationService.GetForCurrentView();
                animation.PrepareToAnimate("DescriptionViewer", DescriptionViewer);
                animation.PrepareToAnimate("TitleViewer", TitleViewer);
            }
        }

        public ProgrammingSubmit()
        {
            InitializeComponent();
        }

        private async void MarkdownTextBlock_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(e.Link));
        }

        private async void MarkdownTextBlock_ImageClicked(object sender, LinkClickedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(e.Link));
        }

        private async void LoadSubmission_Clicked(Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
        {
            LoadPreviousSubmissionTip.IsOpen = false;
            if (parameters is null) return;
            viewModel.Loading = true;
            await Dispatcher.YieldAsync();
            try
            {
                var response = await SubmissionModel.FetchCourseSubmissionListAsync(parameters.CourseId, parameters.AssignmentId);
                if (response.Status != StatusCode.OK)
                {
                    AppModel.ShowMessage?.Invoke(response.Message);
                    return;
                }
                var latestSubmission = response.Data.OrderByDescending(i => i.SubmitAt).FirstOrDefault();
                if (latestSubmission is null) return;
                var submissionDetails =
                    await SubmissionModel.FetchCourseSubmissionAsync<List<ProgrammingAnswer>, object>(parameters.CourseId,
                        parameters.AssignmentId,
                        latestSubmission.SubmissionId);
                if (submissionDetails.Status != StatusCode.OK)
                {
                    AppModel.ShowMessage?.Invoke(response.Message);
                    return;
                }
                var answers = submissionDetails.Data.Answers;
                if (answers is null || viewModel.Files is null) return;

                foreach (var i in viewModel.Files)
                {
                    if (!i.IsSupportFile)
                        i.Content = answers.FirstOrDefault(a => a.Name == i.FileName)?.Code ?? "";
                }

                AppModel.ShowMessage?.Invoke("已加载上次提交内容");
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message, ex.StackTrace);
                AppModel.ShowMessage?.Invoke(ex.Message);
            }
            finally
            {
                viewModel.Loading = false;
            }

        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (parameters is null) return;
            if (viewModel.Files is null) return;

            viewModel.Loading = true;
            await Dispatcher.YieldAsync();

            var content = new SubmitPostModel<List<ProgrammingAnswer>>();
            content.Detail.Answers = new List<ProgrammingAnswer>();
            foreach (var i in viewModel.Files.Where(i => !i.IsSupportFile))
            {
                content.Detail.Answers.Add(new ProgrammingAnswer
                {
                    Name = i.FileName,
                    Code = i.Content
                });
            }

            try
            {
                var response = await SubmissionModel.SubmitForCourseAssignment(parameters.CourseId, parameters.AssignmentId, content);
                AppModel.ShowMessage?.Invoke(response.Message);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message, ex.StackTrace);
                AppModel.ShowMessage?.Invoke(ex.Message);
            }
            finally
            {
                viewModel.Loading = false;
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (parameters is null) return;
            viewModel.Loading = true;
            await Dispatcher.YieldAsync();
            try
            {
                var response = await SubmissionModel.FetchCourseSubmissionListAsync(parameters.CourseId, parameters.AssignmentId);
                if (response.Status != StatusCode.OK)
                {
                    AppModel.ShowMessage?.Invoke(response.Message);
                    return;
                }
                var latestSubmission = response.Data.OrderByDescending(i => i.SubmitAt).FirstOrDefault();
                if (latestSubmission is null) return;
                var submissionDetails =
                    await SubmissionModel.FetchCourseSubmissionAsync<List<ProgrammingAnswer>, object>(parameters.CourseId,
                        parameters.AssignmentId,
                        latestSubmission.SubmissionId);
                if (submissionDetails.Status != StatusCode.OK)
                {
                    AppModel.ShowMessage?.Invoke(response.Message);
                    return;
                }
                var answers = submissionDetails.Data.Answers;
                if (answers is null) return;
                LoadPreviousSubmissionTip.IsOpen = true;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message, ex.StackTrace);
                AppModel.ShowMessage?.Invoke(ex.Message);
            }
            finally
            {
                viewModel.Loading = false;
            }
        }

        private void SetCodeEditorState(UIElement sender, ProgrammingFileModel model)
        {
            var editor = sender.FindChildOfName<CodeEditor>("SingleCodeEditor");
            if (editor != null)
            {
                editor.Options.ReadOnly = null;
                editor.Options.ReadOnly = model.IsSupportFile;
                editor.Text = model.GetContent?.Invoke(model.FileName, model.IsSupportFile);
            }
            if (model.EditorOptions != null)
            {
                model.EditorOptions.ReadOnly = null;
                model.EditorOptions.ReadOnly = model.IsSupportFile;
            }
        }

        private void EditorContainer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is TabView tv)) return;
            var item = e.AddedItems.FirstOrDefault();
            if (!(item is ProgrammingFileModel model)) return;
            SetCodeEditorState(tv, model);
        }

        private void MainCodeEditor_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is CodeEditor editor)) return;
            if (!(EditorContainer.SelectedItem is ProgrammingFileModel model)) return;
            SetCodeEditorState(editor, model);
        }
    }
}
