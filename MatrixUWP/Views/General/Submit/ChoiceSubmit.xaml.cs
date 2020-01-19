﻿#nullable enable
using MatrixUWP.Extensions;
using MatrixUWP.Models;
using MatrixUWP.Models.Submission;
using MatrixUWP.Models.Submission.Answer;
using MatrixUWP.ViewModels;
using MatrixUWP.Views.Parameters.Submit;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        private ChoiceSubmitParameters? parameters;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is ChoiceSubmitParameters p)
            {
                parameters = p;
                viewModel.Description = p.Description;
                viewModel.Questions = p.Questions;
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
                    parameters.ShowMessage(response.Message);
                    return;
                }
                var latestSubmission = response.Data.OrderByDescending(i => i.SubmitAt).FirstOrDefault();
                if (latestSubmission is null) return;
                var submissionDetails =
                    await SubmissionModel.FetchCourseSubmissionAsync<List<ChoiceAnswer>, object>(parameters.CourseId,
                        parameters.AssignmentId,
                        latestSubmission.SubmissionId);
                if (submissionDetails.Status != StatusCode.OK)
                {
                    parameters.ShowMessage(response.Message);
                    return;
                }
                var answers = submissionDetails.Data.Answers;
                if (answers is null) return;

                var load = await LoadPreviousSubmissionDialog.ShowAsync();
                if (load != ContentDialogResult.Primary) return;

                foreach (var i in viewModel.Questions.SelectMany(q => q.Choices, (_, c) => c))
                {
                    i.IsChecked = false;
                }

                foreach (var i in answers)
                {
                    var question = viewModel.Questions.FirstOrDefault(q => q.Id == i.QuestionId);
                    if (question is null || question.Choices is null || i.ChoiceId is null) continue;
                    foreach (var c in question.Choices
                        .Where(x => i.ChoiceId.Contains(x.Id))) c.IsChecked = true;
                }
                parameters.ShowMessage("已加载上次提交内容");
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message, ex.StackTrace);
                parameters.ShowMessage(ex.Message);
            }
            finally
            {
                viewModel.Loading = false;
            }
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (parameters is null) return;
            var content = new SubmitPostModel<List<ChoiceAnswer>>();
            content.Detail.Answers = new List<ChoiceAnswer>();
            foreach (var i in viewModel.Questions)
            {
                content.Detail.Answers.Add(new ChoiceAnswer
                {
                    QuestionId = i.Id,
                    ChoiceId = i.Choices.Where(c => c.IsChecked).Select(c => c.Id).ToList()
                });
            }

            viewModel.Loading = true;
            await Dispatcher.YieldAsync();
            try
            {
                var response = await SubmissionModel.SubmitForCourseAssignment(parameters.CourseId, parameters.AssignmentId, content);
                parameters.ShowMessage(response.Message);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message, ex.StackTrace);
                parameters.ShowMessage(ex.Message);
            }
            finally
            {
                viewModel.Loading = false;
            }
        }
    }
}
