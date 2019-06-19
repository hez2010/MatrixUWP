﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

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
}
