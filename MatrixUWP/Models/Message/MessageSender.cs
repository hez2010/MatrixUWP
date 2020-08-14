#nullable enable
using MatrixUWP.Shared.Utils;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MatrixUWP.Models.Message
{
    public class MessageSender
    {
        private readonly long Ticks = DateTime.Now.Ticks;
        public string DisplayName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public int Id { get; set; }
        public ImageSource Avatar
        {
            get
            {
                var bitmap = new BitmapImage();
                if (Id == 0)
                {
                    bitmap.UriSource = new Uri("ms-appx:///Assets/Home/matrix.png");
                }
                else
                {
                    try
                    {
                        bitmap.UriSource = new Uri(HttpUtils.MatrixHttpClient.BaseUri, $"/api/users/profile/avatar?username={UserName}&t={Ticks}");
                    }
                    catch
                    {
                        bitmap.UriSource = new Uri("ms-appx:///Assets/Home/matrix.png");
                    }
                }

                return bitmap;
            }
        }
        public ObservableCollection<MessageContent> Messages { get; set; } = default!;
    }
}
