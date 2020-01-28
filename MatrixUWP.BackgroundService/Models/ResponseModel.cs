#nullable enable
using Newtonsoft.Json;
using System;

namespace MatrixUWP.BackgroundService.Models
{
    class ResponseModel<T>
    {
        [JsonProperty("status")]
        public StatusCode Status { get; set; }
        [JsonProperty("msg")]
        public string Message { get; set; } = "";
        [JsonProperty("data")]
        public T Data { get; set; } = default!;
        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }
}
