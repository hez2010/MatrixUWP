#if WS_ENABLED
#nullable enable
using MatrixUWP.BackgroundService.Extensions;
using MatrixUWP.BackgroundService.Models.Notification;
using MatrixUWP.BackgroundService.Models.Notification.Content;
using MatrixUWP.Shared.Models;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace MatrixUWP.BackgroundService.Tasks
{
    public sealed class NotificationBackgroundTask : IBackgroundTask
    {
        private static readonly ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();
        public static void PushMessage(string message) => messageQueue.Enqueue(message);

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            while (messageQueue.TryDequeue(out var str))
            {
                try
                {
                    if (string.IsNullOrEmpty(str)) return;
                    var result = str.DeserializeJson<ResponseModel<NotificationModel>>(JsonExtensions.JsonSerializerSettings);
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
}
#endif