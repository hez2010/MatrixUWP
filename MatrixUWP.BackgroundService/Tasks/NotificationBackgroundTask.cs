#nullable enable
using MatrixUWP.BackgroundService.Extensions;
using MatrixUWP.BackgroundService.Models;
using MatrixUWP.BackgroundService.Models.Notification;
using MatrixUWP.BackgroundService.Models.Notification.Content;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Networking.PushNotifications;
using Windows.Networking.Sockets;
using Windows.UI.Notifications;

namespace MatrixUWP.BackgroundService.Tasks
{
    public sealed class NotificationBackgroundTask : IBackgroundTask
    {
        private const string channelId = "MatrixUWP_Notifications";
#if DEBUG
        private static readonly Uri baseUri = new Uri("wss://test.vmatrix.org.cn/");
#else
        private static readonly Uri baseUri = new Uri("wss://vmatrix.org.cn/");
#endif

        private static async Task RegistTaskAsync()
        {
            await UnregistAsync();

            try
            {
                var access = await BackgroundExecutionManager.RequestAccessAsync();
                if (access == BackgroundAccessStatus.DeniedBySystemPolicy ||
                    access == BackgroundAccessStatus.DeniedByUser ||
                    access == BackgroundAccessStatus.Unspecified) return;

                var socketClient = new MessageWebSocket
                {
                    Control =
                    {
                        ReceiveMode = MessageWebSocketReceiveMode.FullMessage,
                        MessageType = SocketMessageType.Utf8
                    }
                };
                var channel = new ControlChannelTrigger(channelId, 15, ControlChannelTriggerResourceType.RequestHardwareSlot);

                var keepAliveBuilder = new BackgroundTaskBuilder
                {
                    Name = nameof(WebSocketKeepAliveTask),
                    TaskEntryPoint = typeof(WebSocketKeepAliveTask).FullName,
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
                builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
                builder.Register();

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
                    .Where(i => i.Value.Name == nameof(NotificationBackgroundTask) || i.Value.Name == nameof(WebSocketKeepAliveTask))
                    .Select(i => i.Value)
                    .ToList();
                foreach (var i in tasks) i.Unregister(true);
            }
            catch (Exception ex)
            {
#if FAIL_ON_DEBUG
                Debug.Fail(ex.Message, ex.StackTrace);
#endif
            }
            return Task.CompletedTask;
        }

        public static IAsyncAction RegistAsync() => RegistTaskAsync().AsAsyncAction();
        public static IAsyncAction UnregistAsync() => UnregistTaskAsync().AsAsyncAction();

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            if (!(taskInstance.TriggerDetails is RawNotification notification)) return;
            try
            {
                var str = notification.Content;
                if (str is null) return;
                var result = str.DeserializeJson<ResponseModel<NotificationModel>>();
                if (result is null) return;

                if (result.Data.Type == "system")
                {
                    var content = result.Data.Content?.ToObject<SystemContent>();
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
                                        Text = content?.Text
                                    },
                                    new AdaptiveText
                                    {
                                        Text = result.Data.Sender?.Name
                                    }
                                }
                        }
                    };

                    var toast = new ToastNotification(new ToastContent { Visual = visual }.GetXml());

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
