﻿#nullable enable
using MatrixUWP.Converters;
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
                if (p.Submissions != null && p.SubmitContents != null && p.Submissions.Count == p.SubmitContents.Count)
                {
                    for (var i = 0; i < p.Submissions.Count; i++)
                    {
                        viewModel.Files.Add(new ProgrammingFileModel
                        {
                            SuppressSetDispatcher = true,
                            Index = i,
                            FileName = p.Submissions[i],
                            EditorOptions = new IEditorConstructionOptions
                            {
                                ReadOnly = true,
                                Language = languageConverter.Convert(p.Languages?.FirstOrDefault()!, typeof(string), null!, null!)?.ToString() ?? ""
                            },
                            NeedsSubmit = true,
                            Content = p.SubmitContents[i],
                            SetContent = p.SetContent,
                            GetContent = p.GetContent
                        });
                    }
                }
                if (p.Supports != null)
                {
                    for (var i = 0; i < p.Supports.Count; i++)
                    {
                        viewModel.Files.Add(new ProgrammingFileModel
                        {
                            Index = i,
                            SuppressSetDispatcher = true,
                            FileName = p.Supports[i],
                            EditorOptions = new IEditorConstructionOptions
                            {
                                ReadOnly = true,
                                Language = languageConverter.Convert(p.Languages?.FirstOrDefault()!, typeof(string), null!, null!)?.ToString() ?? ""
                            },
                            NeedsSubmit = false,
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

        private void LoadSubmission_Clicked(Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
        {

        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void EditorContainer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is TabView tv)) return;
            var item = e.AddedItems.FirstOrDefault();
            if (!(item is ProgrammingFileModel model)) return;
            var editor = tv.FindChildOfName<CodeEditor>("SingleCodeEditor");
            if (editor != null)
            {
                editor.Options.ReadOnly = null;
                editor.Options.ReadOnly = !model.NeedsSubmit;
                editor.Text = model.GetContent?.Invoke(model.Index, !model.NeedsSubmit);
            }
            if (model.EditorOptions != null)
            {
                model.EditorOptions.ReadOnly = null;
                model.EditorOptions.ReadOnly = !model.NeedsSubmit;
            }
        }

        private void MainCodeEditor_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is CodeEditor editor)) return;
            if (!(EditorContainer.SelectedItem is ProgrammingFileModel model)) return;
            if (editor != null)
            {
                editor.Options.ReadOnly = null;
                editor.Options.ReadOnly = !model.NeedsSubmit;
                editor.Text = model.GetContent?.Invoke(model.Index, !model.NeedsSubmit);
            }
            if (model.EditorOptions != null)
            {
                model.EditorOptions.ReadOnly = null;
                model.EditorOptions.ReadOnly = !model.NeedsSubmit;
            }
        }
    }
}
