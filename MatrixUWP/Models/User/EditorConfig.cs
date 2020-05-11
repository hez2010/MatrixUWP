#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.Models.User
{
    public class EditorConfig
    {
        /// <summary>
        /// 双栏编辑器题目信息宽度
        /// </summary>
        [JsonProperty("assignmentInfoWidth")]
        public uint? AssignmentInfoWidth { get; set; }
        /// <summary>
        /// 双栏编辑器代码区域宽度
        /// </summary>
        [JsonProperty("playgroundWidth")]
        public uint? PlaygroundWidth { get; set; }
        /// <summary>
        /// 编辑器浅色/深色主题 ('dark’, ‘light’)
        /// </summary>
        [JsonProperty("codeEditorTheme")]
        public string? CodeEditorTheme { get; set; }
    }
}