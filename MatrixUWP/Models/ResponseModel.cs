#nullable enable
﻿using System;
using Newtonsoft.Json;

namespace MatrixUWP.Models
{
    public class ResponseModel<T>
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
