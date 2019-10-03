﻿using System;
using MatrixUWP.Annotations;
using Newtonsoft.Json;

namespace MatrixUWP.Models
{
    public class ResponseModel<T>
    {
        [JsonProperty("status")]
        public string Status { get; set; } = "";
        [JsonProperty("msg")]
        public string Message { get; set; } = "";
        [JsonProperty("data"), CanBeNull]
        public T Data { get; set; }
        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }
    public class ResponseModel
    {
        [JsonProperty("status")]
        public string Status { get; set; } = "";
        [JsonProperty("msg")]
        public string Message { get; set; } = "";
        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }
}
