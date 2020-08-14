#nullable enable
using MatrixUWP.Models;
using MatrixUWP.Models.Message;
using MatrixUWP.Shared.Models;
using MatrixUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatrixUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Messages : Page, IDisposable
    {
        private readonly MessageViewModel viewModel = new MessageViewModel();
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public Messages()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            cancellationTokenSource.Dispose();
        }

        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var token = cancellationTokenSource.Token;
            viewModel.Loading = true;
            try
            {
                var res = await MessageModel.FetchAnnouncementListAsync();
                if (res is null || res?.Status != StatusCode.OK) throw new Exception("无法获取公告");
                MessageSender? msgSender = null;
                foreach (var i in res.Data)
                {
                    // 确保系统公告的 Sender 只有一个实例
                    if (msgSender is null)
                    {
                        msgSender = i.MessageSender;
                        var collection = new ObservableCollection<MessageContent>
                        {
                            i.MessageContent
                        };
                        msgSender.Messages = collection;
                        viewModel.Messages[msgSender] = collection;
                    }
                    else
                    {
                        viewModel.Messages[msgSender].Add(i.MessageContent);
                    }
                }
            }
            catch (Exception ex)
            {
                AppModel.ShowMessage?.Invoke(ex.Message);
            }

            var pageId = 1;
            var msgSenders = new Dictionary<(int, string), MessageSender>();

            while (true)
            {
                if (token.IsCancellationRequested) break;
                try
                {
                    var res = await MessageModel.FetchNotificationListAsync(pageId);
                    if (res is null || res?.Status != StatusCode.OK) throw new Exception("无法获取消息");
                    if (res.Data.Notifications.Count == 0) break;
                    foreach (var i in res.Data.Notifications)
                    {
                        // 确保同 Id 和 UserName 的 msgSender 是同一个实例
                        // 因此字典中不会出现多个 Id 和 UserName 相同的但是实例不同的 key
                        if (!msgSenders.ContainsKey((i.MessageSender.Id, i.MessageSender.UserName)))
                        {
                            var msgSender = i.MessageSender;
                            msgSenders[(i.MessageSender.Id, i.MessageSender.UserName)] = msgSender;
                            var collection = new ObservableCollection<MessageContent>
                            {
                                i.MessageContent
                            };
                            msgSender.Messages = collection;
                            viewModel.Messages[msgSender] = collection;
                        }
                        else
                        {
                            var msgSender = msgSenders[(i.MessageSender.Id, i.MessageSender.UserName)];
                            viewModel.Messages[msgSender].Add(i.MessageContent);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppModel.ShowMessage?.Invoke(ex.Message);
                }
                pageId++;
            }

            viewModel.Loading = false;
        }

        private void Page_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // 取消未完成的加载
            cancellationTokenSource.Cancel();
        }
    }
}
