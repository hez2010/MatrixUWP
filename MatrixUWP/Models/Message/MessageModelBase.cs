#nullable enable
using Newtonsoft.Json.Linq;
using System;

namespace MatrixUWP.Models.Message
{
    public class MessageContent
    {
        public string Text { get; set; } = default!;
        public DateTime Time { get; set; }
        public string TimeText => Time.ToString("yyyy/MM/dd HH:mm:ss");
        public bool HasRead { get; set; }
        public string Type { get; set; } = default!;
        public int? CourseId { get; set; }
        public int? CourseAssignmentId { get; set; }
        public int? CommentId { get; set; }
        public int? DiscussionId { get; set; }
        public int? ReplyId { get; set; }
        public int Id { get; set; }
        public MessageModelBase OriginalMessage { get; set; } = default!;
    }
    public abstract class MessageModelBase
    {
        public abstract string Type { get; set; }
        public MessageSender MessageSender => this switch
        {
            AnnouncementModel _ => new MessageSender
            {
                Id = 0,
                DisplayName = "系统公告",
                UserName = "matrix"
            },
            NotificationModel nm => new MessageSender
            {
                Id = nm.Sender.Id,
                DisplayName = GetDisplayName(nm.Sender.Name),
                UserName = GetUserName(nm.Sender.Name)
            },
            _ => throw new NotSupportedException()
        };

        public MessageContent MessageContent => this switch
        {
            AnnouncementModel am => new MessageContent
            {
                Text = am.Message,
                Id = am.AnnouncementId,
                Type = "系统通知",
                Time = am.UpdatedAt,
                HasRead = false,
                OriginalMessage = this
            },
            NotificationModel nm => new MessageContent
            {
                Id = nm.Id,
                CommentId = nm.Type switch
                {
                    "course" => null,
                    _ => nm.Content?["link"]?["cmt_id"]?.ToObject<int>()
                },
                HasRead = nm.Status == 1,
                Time = nm.Time,
                Type = nm.Type switch
                {
                    "reply" => "解答",
                    "comment" => "提问",
                    "course" => "课程",
                    "homework" => "作业",
                    "discussion" => "讨论",
                    "system" => "系统",
                    "library" => "题库",
                    _ => "其他"
                },
                CourseAssignmentId = nm.Type switch
                {
                    "course" => null,
                    _ => nm.Content?["link"]?["ca_id"]?.ToObject<int>()
                },
                CourseId = nm.Type switch
                {
                    "course" => nm.Content?["link"]?.ToObject<int>(),
                    _ => nm.Content?["link"]?["cmt_id"]?.ToObject<int>()
                },
                DiscussionId = nm.Type switch
                {
                    "course" => null,
                    _ => nm.Content?["link"]?["dis_id"]?.ToObject<int>()
                },
                ReplyId = nm.Type switch
                {
                    "course" => null,
                    _ => nm.Content?["link"]?["rep_id"]?.ToObject<int>()
                },
                Text = nm.Type switch
                {
                    "reply" => "你在公开课题目下的疑问被一位 TA 或老师解答了",
                    "comment" => "有学生在你参与管理的公开课的题目中提出了疑问",
                    "course" => nm.Content?["text"]?.ToString() ?? "消息内容不见了",
                    "homework" => $"你的作业题目 {nm.Content?["prob_title"] ?? "未知"} 被{nm.Content?["action"] ?? "操作"}",
                    "discussion" => GetDiscussionText(nm.Content),
                    "system" => nm.Content?["text"]?.ToString() ?? "消息内容不见了",
                    "library" => $"题目 {nm.Content?["prob_title"] ?? "未知"} 在题库 {nm.Content?["library_name"] ?? "未知"} 中被{nm.Content?["action"] ?? "操作"}",
                    _ => "消息内容不见了"
                },
                OriginalMessage = this
            },
            _ => throw new NotSupportedException()
        };

        private string GetDiscussionText(JToken? content)
        {
            if (content is null) return "消息内容不见了";
            var title = content["title"]?.ToString();
            var text = content["text"]?.ToString();
            if (string.IsNullOrEmpty(title))
            {
                if (string.IsNullOrEmpty(text))
                {
                    return "消息内容不见了";
                }
                else
                {
                    return text!;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(text))
                {
                    return title!;
                }
                else
                {
                    return title + "\n" + text;
                }
            }
        }

        private string GetDisplayName(JToken? name)
        {
            if (name is null) return "matrix";
            if (name.Type == JTokenType.String) return name.ToString();

            return string.IsNullOrEmpty(name?["nickname"]?.ToString())
                    ? string.IsNullOrEmpty(name?["username"]?.ToString())
                        ? string.IsNullOrEmpty(name?["name"]?.ToString())
                            ? "matrix" : name!["name"]!.ToString()
                        : name!["username"]!.ToString()
                    : name!["nickname"]!.ToString();
        }

        private string GetUserName(JToken? name)
        {
            if (name is null || name.Type == JTokenType.String) return "matrix";

            return string.IsNullOrEmpty(name?["username"]?.ToString())
                    ? string.IsNullOrEmpty(name?["name"]?.ToString())
                        ? "matrix" : name!["name"]!.ToString()
                    : name!["username"]!.ToString();
        }
    }
}
