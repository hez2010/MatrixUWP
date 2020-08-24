#nullable enable
using MatrixUWP.Models.Submission.Programming;
using MatrixUWP.Shared.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MatrixUWP.Utils
{
    class ProgrammingReportUtils
    {
        public static async ValueTask<ProgrammingSubmissionReportModel?> RefactorProgrammingReportAsync(WebView webView, JToken rawReport, JToken config)
        {
            var result = await webView.InvokeScriptAsync("refactorReportFromUWP", new[] { rawReport.ToString(), config.ToString() });
            if (result is null)
            {
                throw new InvalidOperationException("成绩报告转换失败");
            }
            return result.DeserializeJson<ProgrammingSubmissionReportModel>();
        }
    }
}
