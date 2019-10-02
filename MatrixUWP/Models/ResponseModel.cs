using System;
using Newtonsoft.Json;

namespace MatrixUWP.Models
{
    class ResponseModel<T>
    {
        [JsonProperty("status")]
        public string Status { get; set; } = "";
        [JsonProperty("msg")]
        public string Message { get; set; } = "";
        [JsonProperty("data")]
        public T Data { get; set; }
        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }
    class ResponseModel
    {
        [JsonProperty("status")]
        public string Status { get; set; } = "";
        [JsonProperty("msg")]
        public string Message { get; set; } = "";
        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }
}
