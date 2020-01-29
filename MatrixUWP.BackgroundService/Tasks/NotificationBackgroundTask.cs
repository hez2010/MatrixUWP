#nullable enable
using MatrixUWP.BackgroundService.Extensions;
using MatrixUWP.BackgroundService.Models;
using MatrixUWP.BackgroundService.Models.Notification;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Networking.Sockets;
using Windows.UI.Notifications;

namespace MatrixUWP.BackgroundService.Tasks
{
    public sealed class NotificationBackgroundTask : IBackgroundTask
    {
        private static MessageWebSocket? socketClient;
        private static ControlChannelTrigger? channel;
        private const string channelId = "MatrixUWP_Notifications";

#if DEBUG
        private static readonly Uri baseUri = new Uri("wss://test.vmatrix.org.cn/");
#else
        private static readonly Uri baseUri = new Uri("wss://vmatrix.org.cn/");
#endif
        private static readonly ConcurrentQueue<ResponseModel<NotificationModel>> messageQueue = new ConcurrentQueue<ResponseModel<NotificationModel>>();

        private static async Task RegistTaskAsync()
        {
            await UnregistAsync();
            try
            {
                var access = await BackgroundExecutionManager.RequestAccessAsync();
                if (access == BackgroundAccessStatus.DeniedBySystemPolicy ||
                    access == BackgroundAccessStatus.DeniedByUser ||
                    access == BackgroundAccessStatus.Unspecified) return;
                var type = typeof(WebSocketKeepAlive);
                socketClient = new MessageWebSocket();
                socketClient.MessageReceived += SocketClient_MessageReceived;

                channel = new ControlChannelTrigger(channelId, 15, ControlChannelTriggerResourceType.RequestHardwareSlot);

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
#if FAIL_ON_DEBUG
                    Debug.Fail($"Create channel for {builder.Name} falied.");
#endif
                }
            }
            catch (Exception ex)
            {
#if FAIL_ON_DEBUG
                Debug.Fail(ex.Message, ex.StackTrace);
#endif
            }
        }

        private static Task UnregistTaskAsync()
        {
            try
            {
                var tasks = BackgroundTaskRegistration.AllTasks
                    .Where(i => i.Value.Name == nameof(NotificationBackgroundTask) || i.Value.Name == nameof(WebSocketKeepAlive))
                    .Select(i => i.Value)
                    .ToList();
                foreach (var i in tasks) i.Unregister(false);
                socketClient?.Dispose();
            }
            catch (Exception ex)
            {
#if FAIL_ON_DEBUG
                Debug.Fail(ex.Message, ex.StackTrace);
#endif
            }
            finally
            {
                socketClient = null;
            }
            return Task.CompletedTask;
        }

        public static IAsyncAction RegistAsync() => RegistTaskAsync().AsAsyncAction();
        public static IAsyncAction UnregistAsync() => UnregistTaskAsync().AsAsyncAction();

        private static void SocketClient_MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            if (args.IsMessageComplete)
            {
                try
                {
                    using var reader = args.GetDataReader();
                    if (reader.UnconsumedBufferLength <= 0) return;
                    var obj = reader.ReadString(reader.UnconsumedBufferLength).DeserializeJson<ResponseModel<NotificationModel>>();
                    if (obj != null) messageQueue.Enqueue(obj);
                }
                catch (Exception ex)
                {
#if FAIL_ON_DEBUG
                    Debug.Fail(ex.Message, ex.StackTrace);
#endif
                }
            }
        }

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            while (messageQueue.TryDequeue(out var result))
            {
                try
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
                                    Text = result.Data.Content?.Text
                                },
                                new AdaptiveText
                                {
                                    Text = result.Data.Sender?.Name
                                }
                            }
                            }
                        };

                        var content = new ToastContent
                        {
                            Visual = visual
                        };

                        var toast = new ToastNotification(content.GetXml());

                        ToastNotificationManager.CreateToastNotifier().Show(toast);
                    }
                }
                catch (Exception ex)
                {
#if FAIL_ON_DEBUG
                    Debug.Fail(ex.Message, ex.StackTrace);
#endif
                }
            }
        }
    }
}
