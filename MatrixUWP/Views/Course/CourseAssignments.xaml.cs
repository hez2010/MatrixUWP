#nullable enable
using MatrixUWP.Models;
using MatrixUWP.Models.Course.Assignment;
using MatrixUWP.Models.Course.Assignment.Answer;
using MatrixUWP.Models.Course.Assignment.Choice;
using MatrixUWP.Models.Course.Assignment.File;
using MatrixUWP.Models.Course.Assignment.Output;
using MatrixUWP.Models.Course.Assignment.Programming;
using MatrixUWP.Models.Course.Assignment.Report;
using MatrixUWP.Models.Submission;
using MatrixUWP.Models.Submission.Course;
using MatrixUWP.Models.Submission.Programming;
using MatrixUWP.Parameters.Course;
using MatrixUWP.Parameters.Submit;
using MatrixUWP.Shared.Extensions;
using MatrixUWP.Shared.Models;
using MatrixUWP.Utils;
using MatrixUWP.ViewModels;
using MatrixUWP.Views.Submit;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatrixUWP.Views.Course
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

        private void TryJumpToAssignment()
        {
            if ((parameters?.JumpAssignmentId) == null) return;
            var item = viewModel.Assignments.FirstOrDefault(i => i.AssignmentId == parameters.JumpAssignmentId);
            AssignmentView.SelectedItem = item;
            AssignmentView.FindChildOfType<ListView>()?.ScrollIntoView(item);
        }

        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (parameters is null)
            {
                viewModel.Assignments = null;
                return;
            }
            if (LastCourseId == parameters.CourseId)
            {
                TryJumpToAssignment();
                return;
            }
            viewModel.Assignments = null;
            viewModel.Loading = true;

            try
            {
                var response = await CourseAssignmentModel.FetchCourseAssignmentListAsync(parameters.CourseId);
                if (response?.Status != StatusCode.OK)
                {
                    AppModel.ShowMessage?.Invoke(response?.Message ?? "课程作业列表获取失败");
                    return;
                }
                viewModel.Assignments = response.Data
                    .OrderBy(i => i.Expired)
                    .ThenBy(i => i.Finished)
                    .ThenByDescending(i => i.StartTime)
                    .ThenBy(i => i.EndTime)
                    .ToList();

                TryJumpToAssignment();
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

        private async void AssignmentView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0 || parameters is null) return;
            if (!(e.AddedItems.First() is CourseAssignmentDetailsModel selectedItem)) return;
            Rating.Value = selectedItem.Rate;
            Star.IsChecked = selectedItem.Favorited;
            if (selectedItem.Loaded) return;
            selectedItem.Loading = true;
            try
            {
                var response =
                    await CourseAssignmentModel.FetchCourseAssignmentAsync(
                        parameters.CourseId,
                        selectedItem.CourseAssignmentId);
                if (response?.Status != StatusCode.OK)
                {
                    AppModel.ShowMessage?.Invoke(response?.Message ?? "课程作业获取失败");
                    return;
                }
                response.Data.CopyTo(selectedItem);
                selectedItem.Loaded = true;
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
                selectedItem.Loading = false;
            }
        }
        private async void MarkdownTextBlock_LinkClicked(object sender, LinkClickedEventArgs e) => await Windows.System.Launcher.LaunchUriAsync(new Uri(e.Link));

        private async void MarkdownTextBlock_ImageClicked(object sender, LinkClickedEventArgs e) => await Windows.System.Launcher.LaunchUriAsync(new Uri(e.Link));

        private async Task ShowSubmitPage(object config, CourseAssignmentDetailsModel model)
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
                        // 设置文件内容的函数
                        void SetContent(string fileName, string content) => asgnConfig.SubmitContents[fileName] = content;

                        // 获取文件内容的函数
                        string GetContent(string fileName, bool isSupportFile /* 是否是支持文件 */) => isSupportFile switch
                        {
                            true => (asgnConfig.SupportContents?.ContainsKey(fileName) ?? false) ? asgnConfig.SupportContents[fileName] : "",
                            false => asgnConfig.SubmitContents.ContainsKey(fileName) ? asgnConfig.SubmitContents[fileName] : ""
                        };

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
                                RemainingSubmitTimes = model.SubmitLimit == 0 ? -1 : model.SubmitLimit - model.SubmitTimes,
                                Title = model.Title,
                                Description = model.Description,
                                CourseId = model.CourseId,
                                AssignmentId = model.CourseAssignmentId,
                                GetContent = GetContent,
                                SetContent = SetContent
                            }, -1);
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
                            AssignmentId = model.CourseAssignmentId,
                            RemainingSubmitTimes = model.SubmitLimit == 0 ? -1 : model.SubmitLimit - model.SubmitTimes,
                        }, -1);
                    break;
                case ReportAssignmentConfig asgnConfig:
                    AppModel.ShowMessage?.Invoke($"不支持的题目配置:\n{asgnConfig.SerializeJson()}");
                    break;
                case FileAssignmentConfig asgnConfig:
                    {
                        var filePicker = new FileOpenPicker();
                        filePicker.FileTypeFilter.Add("*");
                        var file = await filePicker.PickSingleFileAsync();
                        if (file is null) break;
                        if (file.Name != asgnConfig.FileName)
                        {
                            AppModel.ShowMessage?.Invoke($"文件名不正确，命名格式为：{asgnConfig.FileName}");
                            break;
                        }
                        viewModel.Loading = true;
                        try
                        {
                            var response = await SubmissionModel.SubmitFileForCourseAssignment(model.CourseId, model.CourseAssignmentId, file);
                            AppModel.ShowMessage?.Invoke(response?.Message ?? "提交成功");
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
                        break;
                    }
                case OutputAssignmentConfig asgnConfig:
                    AppModel.ShowMessage?.Invoke($"不支持的题目配置:\n{asgnConfig.SerializeJson()}");
                    break;
                case AnswerAssignmentConfig asgnConfig:
                    AppModel.ShowMessage?.Invoke($"不支持的题目配置:\n{asgnConfig.SerializeJson()}");
                    break;
                default:
                    AppModel.ShowMessage?.Invoke($"不支持的题目配置: 未知");
                    break;
            }
        }

        private async void Submit_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
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
                        5 => throw new NotSupportedException("不支持空白填充题目"),
                        6 => typeof(AnswerAssignmentConfig),
                        _ => throw new NotSupportedException($"不支持的题目类型: ${model.Type}(${model.ProblemTypeId})")
                    };
                }

                if (model.DeserialzedConfig is null)
                    model.DeserialzedConfig = model.Config.ToObject(model.ConfigType, jsonSerializer);
                if (model.DeserialzedConfig is null)
                {
                    AppModel.ShowMessage?.Invoke("题目配置错误");
                    return;
                }
                await ShowSubmitPage(model.DeserialzedConfig, model);
            }
            catch (Exception ex)
            {
#if FAIL_ON_DEBUG
                Debug.Fail(ex.Message, ex.StackTrace);
#endif
                AppModel.ShowMessage?.Invoke(ex.Message);
            }
        }

        private async void SubmitRate_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (!(AssignmentView.SelectedItem is CourseAssignmentDetailsModel model)) return;
            model.Rate = Rating.Value == -1 ? 0 : (int)Rating.Value;
            try
            {
                var res = await CourseAssignmentModel.RateAssignmentAsync(Star.IsChecked ?? false, model.CourseId, model.CourseAssignmentId, model.Rate);
                if (res?.Status != StatusCode.OK)
                {
                    throw new Exception(res?.Message ?? "请求失败");
                }
            }
            catch (Exception ex)
            {
                AppModel.ShowMessage?.Invoke(ex.Message);
            }
            RateFlyout.Hide();
        }

        private async void RankFlyout_Opening(object sender, object e)
        {
            if (!(AssignmentView.SelectedItem is CourseAssignmentDetailsModel model)) return;
            viewModel.RankInfo = null;
            viewModel.LoadingRank = true;
            try
            {
                var res = await CourseAssignmentModel.FetchRankInfoAsync(model.CourseId, model.CourseAssignmentId);
                if (res is null || res.Status != StatusCode.OK)
                {
                    throw new Exception(res?.Message ?? "排名信息加载失败");
                }
                viewModel.RankInfo = res.Data;
            }
            catch (Exception ex)
            {
                AppModel.ShowMessage?.Invoke(ex.Message);
            }
            viewModel.LoadingRank = false;
        }

        private async void ReportFlyout_Opened(object sender, object e)
        {
            if (!(AssignmentView.SelectedItem is CourseAssignmentDetailsModel model)) return;
            viewModel.SubmissionInfo = null;
            viewModel.LoadingSubmission = true;
            try
            {
                var res = await SubmissionModel.FetchCourseSubmissionListAsync(model.CourseId, model.CourseAssignmentId);
                if (res is null || res.Status != StatusCode.OK)
                {
                    throw new Exception(res?.Message ?? "提交记录加载失败");
                }
                viewModel.SubmissionInfo = res.Data;
            }
            catch (Exception ex)
            {
                AppModel.ShowMessage?.Invoke(ex.Message);
            }
            viewModel.LoadingSubmission = false;
        }

        private async void ReportView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(ReportView.SelectedItem is CourseSubmissionInfoModel submission)) return;
            if (!(AssignmentView.SelectedItem is CourseAssignmentDetailsModel assignment)) return;

            submission.LoadingReport = true;
            try
            {
                var res = await SubmissionModel.FetchCourseSubmissionAsync<List<ProgrammingAnswer>, JToken>(assignment.CourseId, assignment.CourseAssignmentId, submission.SubmissionId);
                if (res?.Data.Report is null || res.Status != StatusCode.OK)
                {
                    throw new Exception(res?.Message ?? "成绩报告加载失败");
                }

                var report = new List<SubmissionReportModel>();

                foreach (var check in res.Data.Report.Children())
                {
                    if (!(check is JProperty p)) continue;
                    var grade = p.Value["grade"]?.ToObject<int>() ?? 0;
                    var contin = p.Value["continue"]?.ToObject<bool>() ?? false;
                    var details = p.Value[p.Name];
                    if (details is null) continue;

                    var name = p.Name switch
                    {
                        "memory check" => "内存检查",
                        "static check" => "静态检查",
                        "compile check" => "编译检查",
                        "standard tests" => "标准测试",
                        "random tests" => "随机测试",
                        "google tests" => "谷歌测试",
                        _ => p.Name.Replace("check", "检查").Replace("tests", "测试")
                    };

                    report.Add(new SubmissionReportModel
                    {
                        Name = name,
                        Grade = grade,
                        Logs = details?.ToString(),
                        Pass = contin
                    });
                }

                submission.Report = report;
            }
            catch (Exception ex)
            {
                AppModel.ShowMessage?.Invoke(ex.Message);
            }
            submission.LoadingReport = false;
        }
    }
}
