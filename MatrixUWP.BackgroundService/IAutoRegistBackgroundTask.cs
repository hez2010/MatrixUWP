#nullable enable
using Windows.ApplicationModel.Background;

namespace MatrixUWP.BackgroundService
{
    public interface IAutoRegistBackgroundTask : IBackgroundTask
    {
        public IBackgroundCondition[] GetConditions();
        public IBackgroundTrigger[] GetTriggers();
    }
}
