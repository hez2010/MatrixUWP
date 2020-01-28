#nullable enable
using MatrixUWP.BackgroundService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace MatrixUWP.Utils
{
    static class BackgroundServiceHelper
    {
        public static readonly List<IBackgroundTaskRegistration> BackgroundTasks = BackgroundTaskRegistration.AllTasks.Values.ToList();

        private static string[] GetUnregistTasks()
        {
            var allTasks = typeof(IAutoRegistBackgroundTask).Assembly
                .GetExportedTypes()
                .Where(i => i.GetInterface(nameof(IAutoRegistBackgroundTask)) != null)
                .Select(i => i.Name);
            var tasks = allTasks.ToDictionary(i => i, i => false);
            foreach (var i in BackgroundTasks)
            {
                tasks[i.Name] = true;
            }
            return tasks.Where(i => !i.Value).Select(i => i.Key).ToArray();
        }

        private static async ValueTask RegistTaskAsync(Type taskType)
        {
            var constructor = taskType.GetConstructor(Array.Empty<Type>());
            var task = (IAutoRegistBackgroundTask)constructor.Invoke(Array.Empty<object>());
            var result = await task.RegistAsync();
            if (result != null)
                BackgroundTasks.Add(result);
        }

        public static ValueTask RegistBackgroundTaskAsync<T>() where T : IAutoRegistBackgroundTask
        {
            return RegistTaskAsync(typeof(T));
        }

        public static async ValueTask RegistBackgroundTasksAsync()
        {
            var tasks = typeof(IAutoRegistBackgroundTask).Assembly
                .GetExportedTypes()
                .Where(i => i.GetInterface(nameof(IAutoRegistBackgroundTask)) != null);
            foreach (var taskType in tasks) await RegistTaskAsync(taskType);
            return;
        }

        public static void UnregistBackgroundTask<T>() where T : IAutoRegistBackgroundTask
        {
            var name = typeof(T).Name;
            var tasks = BackgroundTasks.Where(i => i.Name == name).ToList();
            foreach (var task in tasks)
            {
                task.Unregister(true);
                BackgroundTasks.Remove(task);
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
