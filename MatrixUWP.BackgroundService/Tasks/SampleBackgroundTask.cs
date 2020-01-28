#nullable enable
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace MatrixUWP.BackgroundService.Tasks
{
    public sealed class SampleBackgroundTask : IAutoRegistBackgroundTask
    {
        public IBackgroundCondition[] GetConditions()
        {
            var list = new List<IBackgroundCondition>();
            return list.ToArray();
        }
        public IBackgroundTrigger[] GetTriggers()
        {
            var list = new List<IBackgroundTrigger>
            {
                new TimeTrigger(15, false)
            };
            return list.ToArray();
        }

        private BackgroundTaskDeferral? _deferral;
        private static int count = 0;
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            var visual = new ToastVisual
            {
                BindingGeneric = new ToastBindingGeneric
                {
                    Children =
                    {
                        new AdaptiveText
                        {
                            Text = "Sample background task"
                        },
                        new AdaptiveText
                        {
                            Text = $"{++count} times"
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

            _deferral.Complete();
        }
    }
}
