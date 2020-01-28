#nullable enable
using MatrixUWP.BackgroundService.Extensions;
using MatrixUWP.BackgroundService.Models;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Networking.Sockets;
using Windows.UI.Notifications;

namespace MatrixUWP.BackgroundService.Tasks
{
    public sealed class NotificationBackgroundTask : IAutoRegistBackgroundTask
    {
        private static MessageWebSocket? socketClient;
        private const string channelId = "matrixUwpNotifications";

#if DEBUG
        private static readonly Uri baseUri = new Uri("wss://test.vmatrix.org.cn/");
#else
        private static readonly Uri baseUri = new Uri("wss://vmatrix.org.cn/");
#endif
        private static readonly ConcurrentQueue<ResponseModel<dynamic>> messageQueue = new ConcurrentQueue<ResponseModel<dynamic>>();

        private async Task<BackgroundTaskRegistration?> RegistTaskAsync()
        {
            var type = typeof(WebSocketKeepAlive);
            socketClient = new MessageWebSocket();
            socketClient.MessageReceived += SocketClient_MessageReceived;

            var channel = new ControlChannelTrigger(channelId, 15);

            var keepAliveBuilder = new BackgroundTaskBuilder
            {
                Name = nameof(WebSocketKeepAlive),
                TaskEntryPoint = typeof(WebSocketKeepAlive).FullName,
                IsNetworkRequested = true
            };
            keepAliveBuilder.SetTrigger(channel.KeepAliveTrigger);
            keepAliveBuilder.Register();

            var builder = new BackgroundTaskBuilder
            {
                Name = nameof(NotificationBackgroundTask),
                TaskEntryPoint = typeof(NotificationBackgroundTask).FullName,
                IsNetworkRequested = true
            };

            builder.SetTrigger(channel.PushNotificationTrigger);
            var task = builder.Register();

            channel.UsingTransport(socketClient);
            await socketClient.ConnectAsync(new Uri(baseUri, "/api/notifications"));
            var status = channel.WaitForPushEnabled();
            if (status != ControlChannelTriggerStatus.HardwareSlotAllocated
                && status != ControlChannelTriggerStatus.SoftwareSlotAllocated)
            {
                Debug.Fail($"Create channel for {builder.Name} falied.");
                return null;
            }

            return task;
        }

        private void SocketClient_MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            if (args.IsMessageComplete)
            {
                using var reader = args.GetDataReader();
                var obj = reader.ReadString(reader.UnconsumedBufferLength).DeserializeJson<ResponseModel<dynamic>>();
                messageQueue.Enqueue(obj);
            }
        }

        public IAsyncOperation<BackgroundTaskRegistration?> RegistAsync()
        {
            return RegistTaskAsync().AsAsyncOperation();
        }

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            while (messageQueue.TryDequeue(out var result))
            {
                if (result.Data.Type == "system")
                {
                    var visual = new ToastVisual
                    {
                        BindingGeneric = new ToastBindingGeneric
                        {
                            Children =
                            {
                                new AdaptiveText
                                {
                                    Text = "系统通知"
                                },
                                new AdaptiveText
                                {
                                    Text = result.Data.Content.Text
                                },
                                new AdaptiveText
                                {
                                    Text = result.Data.Sender.Name
                                }
                            }
                        }
                    };

                    var content = new ToastContent
                    {
                        Visual = visual
                    };

                    var toast = new ToastNotification(content.GetXml())
                    {
                        ExpirationTime = DateTime.Now.AddMinutes(15)
                    };

                    ToastNotificationManager.CreateToastNotifier().Show(toast);
                }
            }
            deferral.Complete();
        }
    }
}
