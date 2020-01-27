#nullable enable
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Background;

namespace MatrixUWP.Utils
{
    static class BackgroundServiceHelper
    {
        public static readonly List<IBackgroundTaskRegistration> BackgroundTasks = BackgroundTaskRegistration.AllTasks.Values.ToList();
        private static readonly string[] allTasks =
            new[]
            {
                nameof(BackgroundService.Tasks.SampleBackgroundTask)
            };
        private static readonly string taskPath =
            $"{nameof(MatrixUWP)}.{nameof(BackgroundService)}.{nameof(BackgroundService.Tasks)}";

        private static string[] GetUnregistTasks()
        {
            var tasks = allTasks.ToDictionary(i => i, i => false);
            foreach (var i in BackgroundTasks)
            {
                tasks[i.Name] = true;
            }
            return tasks.Where(i => !i.Value).Select(i => i.Key).ToArray();
        }

        private static IBackgroundTrigger[] GetTriggers(string taskName)
        {
            var list = new List<IBackgroundTrigger>();
            switch (taskName)
            {
                case nameof(BackgroundService.Tasks.SampleBackgroundTask):
                    list.Add(new TimeTrigger(15, false));
                    break;
            }
            return list.ToArray();
        }

        private static IBackgroundCondition[] GetConditions(string taskName)
        {
            var list = new List<IBackgroundCondition>();
            switch (taskName)
            {
                case nameof(BackgroundService.Tasks.SampleBackgroundTask):
                    list.Add(new SystemCondition(SystemConditionType.UserPresent));
                    break;
            }
            return list.ToArray();
        }

        private static void RegistTask(string taskName)
        {
            var builder = new BackgroundTaskBuilder
            {
                Name = taskName,
                TaskEntryPoint = $"{taskPath}.{taskName}"
            };
            foreach (var i in GetTriggers(taskName)) builder.SetTrigger(i);
            foreach (var i in GetConditions(taskName)) builder.AddCondition(i);
            BackgroundTasks.Add(builder.Register());
        }

        public static void RegistBackgroundTasks()
        {
            foreach (var i in GetUnregistTasks())
            {
                RegistTask(i);
            }
        }

        public static void UnregistBackgroundTask(IBackgroundTaskRegistration task)
        {
            task.Unregister(true);
            BackgroundTasks.Remove(task);
        }

        public static void UnregistBackgroundTasks()
        {
            foreach (var i in BackgroundTasks)
            {
                i.Unregister(true);
            }
            BackgroundTasks.Clear();
        }
    }
}
