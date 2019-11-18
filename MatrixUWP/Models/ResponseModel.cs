using System;
using Newtonsoft.Json;

namespace MatrixUWP.Models
{
    public enum StatusCode
    {
        OK, BAD_REQUEST, NOT_FOUND, NOT_AUTHORIZED, WRONG_CAPTCHA, WRONG_PASSWORD
    }

    public class ResponseModel<T>
    {
        [JsonProperty("status")]
        public StatusCode Status { get; set; }
        [JsonProperty("msg")]
        public string Message { get; set; } = "";
        [JsonProperty("data")]
        public T Data { get; set; }
        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }

    public class ResponseModel
    {
        [JsonProperty("status")]
        public StatusCode Status { get; set; }
        [JsonProperty("msg")]
        public string Message { get; set; } = "";
        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }
}
