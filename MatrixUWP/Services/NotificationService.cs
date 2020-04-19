#nullable enable
using MatrixUWP.BackgroundService.Tasks;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Networking.Sockets;

namespace MatrixUWP.Services
{
    internal static class NotificationService
    {
        private const string channelId = "MatrixUWP_Notifications";
#if DEBUG
        private static readonly Uri baseUri = new Uri("wss://test.vmatrix.org.cn/");
#else
#warning Restore uri before publishing stable version of Matrix UWP
        private static readonly Uri baseUri = new Uri("wss://test.vmatrix.org.cn/");
#endif
        public static void UnregistTask()
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
        }

        public static async Task<bool> RegistTaskAsync()
        {
            UnregistTask();

            var access = await BackgroundExecutionManager.RequestAccessAsync();
            if (access == BackgroundAccessStatus.DeniedBySystemPolicy ||
                access == BackgroundAccessStatus.DeniedByUser ||
                access == BackgroundAccessStatus.Unspecified)
            {
                return false;
            }

            try
            {
                var socketClient = new MessageWebSocket
                {
                    Control =
                    {
                        ReceiveMode = MessageWebSocketReceiveMode.FullMessage,
                        MessageType = SocketMessageType.Utf8
                    }
                };
                socketClient.MessageReceived += (sender, args) =>
                {
                    if (!args.IsMessageComplete) return;
                    try
                    {
                        using var reader = args.GetDataReader();
                        NotificationBackgroundTask.PushMessage(reader.ReadString(reader.UnconsumedBufferLength));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
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

                await socketClient.ConnectAsync(new Uri(baseUri, "/ws"));
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
                return false;
            }
            return true;
        }
    }
}
