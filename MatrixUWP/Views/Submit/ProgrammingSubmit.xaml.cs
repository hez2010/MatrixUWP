#nullable enable
using MatrixUWP.Converters;
using MatrixUWP.Models;
using MatrixUWP.Models.Submission;
using MatrixUWP.Models.Submission.Programming;
using MatrixUWP.Parameters.Submit;
using MatrixUWP.Shared.Models;
using MatrixUWP.ViewModels;
using MatrixUWP.Views.Course;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatrixUWP.Views.Submit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProgrammingSubmit : Page
    {
        private readonly ProgrammingSubmitViewModel viewModel = new ProgrammingSubmitViewModel();
        private ProgrammingSubmitParameters? parameters;

        /// <summary>
        /// 根据文件名获取语言名称
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetCodeLanguage(string? fileName)
        {
            if (fileName is null) return "";
            var extName = System.IO.Path.GetExtension(fileName).ToLowerInvariant();
            return extName switch
            {
                ".c" => "cpp",
                ".cpp" => "cpp",
                ".h" => "cpp",
                ".hpp" => "cpp",
                ".py" => "python",
                ".cs" => "csharp",
                ".hs" => "haskell",
                ".fs" => "fsharp",
                ".rs" => "rust",
                ".kt" => "kotlin",
                ".js" => "javascript",
                ".ts" => "typescript",
                _ => extName.Length == 0 ? "" : extName.Substring(1)
            };
        }

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
                viewModel.RemainingSubmitTimes = p.RemainingSubmitTimes;
                viewModel.Files = new List<ProgrammingFileModel>();

                if (p.Submissions != null)
                {
                    foreach (var i in p.Submissions)
                    {
                        viewModel.Files.Add(new ProgrammingFileModel
                        {
                            FileName = i,
                            Options =
                            {
                                ReadOnly = false,
                                Language = GetCodeLanguage(i)
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
                            FileName = i,
                            Options =
                            {
                                ReadOnly = true,
                                Language = GetCodeLanguage(i)
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

        private async void MarkdownTextBlock_LinkClicked(object sender, LinkClickedEventArgs e) => await Windows.System.Launcher.LaunchUriAsync(new System.Uri(e.Link));

        private async void MarkdownTextBlock_ImageClicked(object sender, LinkClickedEventArgs e) => await Windows.System.Launcher.LaunchUriAsync(new System.Uri(e.Link));

        private async void LoadSubmission_Clicked(Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
        {
            LoadPreviousSubmissionTip.IsOpen = false;
            if (parameters is null) return;
            viewModel.Loading = true;

            try
            {
                var response = await SubmissionModel.FetchCourseSubmissionListAsync(parameters.CourseId, parameters.AssignmentId);
                if (response?.Status != StatusCode.OK)
                {
                    AppModel.ShowMessage?.Invoke(response?.Message ?? "课程提交列表获取失败");
                    return;
                }
                var latestSubmission = response.Data.OrderByDescending(i => i.SubmitAt).FirstOrDefault();
                if (latestSubmission is null) return;
                var submissionDetails =
                    await SubmissionModel.FetchCourseSubmissionAsync<List<ProgrammingAnswer>, object>(parameters.CourseId,
                        parameters.AssignmentId,
                        latestSubmission.SubmissionId);
                if (submissionDetails?.Status != StatusCode.OK)
                {
                    AppModel.ShowMessage?.Invoke(response?.Message ?? "课程提交获取失败");
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
#if FAIL_ON_DEBUG
                Debug.Fail(ex.Message, ex.StackTrace);
#endif
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
                AppModel.ShowMessage?.Invoke(response?.Message ?? "发生错误");
            }
            catch (Exception ex)
            {
#if FAIL_ON_DEBUG
                Debug.Fail(ex.Message, ex.StackTrace);
#endif
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

            try
            {
                var response = await SubmissionModel.FetchCourseSubmissionListAsync(parameters.CourseId, parameters.AssignmentId);
                if (response?.Status != StatusCode.OK)
                {
                    AppModel.ShowMessage?.Invoke(response?.Message ?? "课程提交列表获取失败");
                    return;
                }
                var latestSubmission = response.Data.OrderByDescending(i => i.SubmitAt).FirstOrDefault();
                if (latestSubmission is null) return;
                var submissionDetails =
                    await SubmissionModel.FetchCourseSubmissionAsync<List<ProgrammingAnswer>, object>(parameters.CourseId,
                        parameters.AssignmentId,
                        latestSubmission.SubmissionId);
                if (submissionDetails?.Status != StatusCode.OK)
                {
                    AppModel.ShowMessage?.Invoke(response?.Message ?? "课程提交获取失败");
                    return;
                }
                var answers = submissionDetails.Data.Answers;
                if (answers is null) return;
                LoadPreviousSubmissionTip.IsOpen = true;
            }
            catch (Exception ex)
            {
#if FAIL_ON_DEBUG
                Debug.Fail(ex.Message, ex.StackTrace);
#endif
                AppModel.ShowMessage?.Invoke(ex.Message);
            }
            finally
            {
                viewModel.Loading = false;
            }
        }
    }
}
