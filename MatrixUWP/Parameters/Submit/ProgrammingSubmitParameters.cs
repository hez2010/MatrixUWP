#nullable enable
using System;
using System.Collections.Generic;

namespace MatrixUWP.Parameters.Submit
{
    internal class ProgrammingSubmitParameters
    {
        public string Description { get; set; } = "";
        public string Title { get; set; } = "";
        public int CourseId { get; set; }
        public int AssignmentId { get; set; }
        /// <summary>
        /// 需要提交的文件
        /// </summary>
        public List<string>? Submissions { get; set; }
        /// <summary>
        /// 支持文件
        /// </summary>
        public List<string>? Supports { get; set; }
        /// <summary>
        /// 获取代码内容的函数指针，通过此方法获取 asgnConfig.SubmitContents 中的文件内容
        /// string GetContent(string fileName, bool isSupportFile)
        /// </summary>
        public Func<string, bool, string>? GetContent { get; set; }
        /// <summary>
        /// 设置代码内容的函数指针，通过此方法设置 asgnConfig.SubmitContents 中的文件内容
        /// void SetContent(string fileName, string content)
        /// </summary>
        public Action<string, string>? SetContent { get; set; }
        public int RemainingSubmitTimes { get; set; }
    }
}
