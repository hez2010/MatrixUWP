using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MatrixUWP.Models
{
    class ResponseModel<T>
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = "";
        [JsonPropertyName("msg")]
        public string Message { get; set; } = "";
        [JsonPropertyName("data")]
        public T Data { get; set; }
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }
    }
}
