#nullable enable
using MatrixUWP.Shared.Extensions;
using MatrixUWP.Shared.Models;
using MatrixUWP.Shared.Utils;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Networking.PushNotifications;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.UI.Notifications;

namespace MatrixUWP.BackgroundService.Tasks
{
    public sealed class PushServiceBackgroundTask : IBackgroundTask
    {
        private static PushNotificationChannel? _channel;
        private static readonly ConcurrentQueue<(PushNotificationType Type, object Notification)> messageQueue = new ConcurrentQueue<(PushNotificationType, object)>();
        public static IAsyncOperation<bool> CreateChannelAsync(long userId)
        {
            return InternalCreateChannelAsync(userId).AsAsyncOperation();
        }

        private static void OnReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            messageQueue.Enqueue((args.NotificationType, args.NotificationType switch
            {
                PushNotificationType.Toast => args.ToastNotification,
                PushNotificationType.Badge => args.BadgeNotification,
                PushNotificationType.Tile => args.TileNotification,
                PushNotificationType.TileFlyout => args.TileNotification,
                PushNotificationType.Raw => args.RawNotification,
                _ => throw new NotSupportedException($"Notification type ${args.NotificationType} is not supported.")
            }));
        }

        private static async Task<bool> InternalCreateChannelAsync(long userId)
        {
            try
            {
                if (_channel != null)
                {
                    _channel.PushNotificationReceived -= OnReceived;
                    _channel.Close();
                }
                _channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                _channel.PushNotificationReceived += OnReceived;
                var sysInfo = new EasClientDeviceInformation();
                var sysId = sysInfo.Id.ToString();
                var response =
                    await HttpUtils.MatrixHttpClient.PostJsonAsync(
                        "/api/wns/regist",
                        new
                        {
                            user_id = userId,
                            channel_uri = _channel.Uri,
                            device_id = sysId,
                            expire_time = _channel.ExpirationTime.DateTime
                        })
                    .JsonAsync<ResponseModel>();
                return response.Status == StatusCode.OK;
            }
            catch (Exception ex)
            {
#if FAIL_ON_DEBUG
                Debug.Fail(ex.Message, ex.StackTrace);
#endif
                return false;
            }
        }

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            while (messageQueue.TryDequeue(out var item))
            {
                var (type, notification) = item;
                switch (type)
                {
                    case PushNotificationType.Toast:
                        if (notification is ToastNotification toast)
                        {
                            ToastNotificationManager.CreateToastNotifier().Show(toast);
                        }
                        break;
                }
            }
        }
    }
}
