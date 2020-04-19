#nullable enable
using Windows.ApplicationModel.Background;
using Windows.Networking.Sockets;

namespace MatrixUWP.BackgroundService.Tasks
{
    public sealed class WebSocketKeepAliveTask : IBackgroundTask
    {
        private readonly WebSocketKeepAlive innerTask = new WebSocketKeepAlive();
        public void Run(IBackgroundTaskInstance taskInstance) => innerTask.Run(taskInstance);
    }
}
