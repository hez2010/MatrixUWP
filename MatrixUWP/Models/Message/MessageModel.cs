#nullable enable
using MatrixUWP.Shared.Extensions;
using MatrixUWP.Shared.Models;
using MatrixUWP.Shared.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixUWP.Models.Message
{
    public class MessageModel
    {
        public static async ValueTask<ResponseModel<NotificationResponseModel>?> FetchNotificationListAsync(int pageId, int count = 100)
            => await HttpUtils.MatrixHttpClient.GetAsync($"/api/notifications?page_id={pageId}&page_size={count}").JsonAsync<ResponseModel<NotificationResponseModel>>();

        public static async ValueTask<ResponseModel<List<AnnouncementModel>>?> FetchAnnouncementListAsync()
            => await HttpUtils.MatrixHttpClient.GetAsync("/api/announcements").JsonAsync<ResponseModel<List<AnnouncementModel>>>();
    }
}
