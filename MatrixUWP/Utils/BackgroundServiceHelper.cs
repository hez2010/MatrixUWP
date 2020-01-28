#nullable enable
using MatrixUWP.BackgroundService;
using System;
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

        private static void RegistTask(Type taskType)
        {
            var constructor = taskType.GetConstructor(Array.Empty<Type>());
            var task = (IAutoRegistBackgroundTask)constructor.Invoke(Array.Empty<object>());
            var builder = new BackgroundTaskBuilder
            {
                Name = taskType.Name,
                TaskEntryPoint = taskType.FullName
            };
            foreach (var trigger in task.GetTriggers()) builder.SetTrigger(trigger);
            foreach (var condition in task.GetConditions()) builder.AddCondition(condition);

            BackgroundTasks.Add(builder.Register());
        }

        public static void RegistBackgroundTask<T>() where T : IAutoRegistBackgroundTask
        {
            RegistTask(typeof(T));
        }

        public static void RegistBackgroundTasks()
        {
            var tasks = typeof(IAutoRegistBackgroundTask).Assembly
                .GetExportedTypes()
                .Where(i => i.GetInterface(nameof(IAutoRegistBackgroundTask)) != null);
            foreach (var taskType in tasks) RegistTask(taskType);
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
