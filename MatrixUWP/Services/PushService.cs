#nullable enable
using MatrixUWP.BackgroundService.Tasks;
using MatrixUWP.Models.User;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace MatrixUWP.Services
{
    class PushService
    {
        public static void UnregistTask()
        {
            try
            {
                var tasks = BackgroundTaskRegistration.AllTasks
                    .Where(i => i.Value.Name == nameof(PushServiceBackgroundTask))
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

            if (!await PushServiceBackgroundTask.CreateChannelAsync(UserModel.CurrentUser.UserId)) return false;

            try
            {
                var builder = new BackgroundTaskBuilder
                {
                    Name = nameof(PushServiceBackgroundTask),
                    TaskEntryPoint = typeof(PushServiceBackgroundTask).FullName,
                    IsNetworkRequested = true
                };

                builder.SetTrigger(new PushNotificationTrigger());
                builder.Register();
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
