#nullable enable
using Windows.ApplicationModel.Background;
using Windows.Foundation;

namespace MatrixUWP.BackgroundService
{
    public interface IAutoRegistBackgroundTask : IBackgroundTask
    {
        IAsyncOperation<BackgroundTaskRegistration?> RegistAsync();
        IAsyncAction UnregistAsync();
    }
}
