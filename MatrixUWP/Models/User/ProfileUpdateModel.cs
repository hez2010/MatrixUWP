#nullable enable
﻿using Newtonsoft.Json;

namespace MatrixUWP.Models.User
{
    public class ProfileUpdateModel
    {
        [JsonProperty("email")]
        public string Email { get; set; } = "";
        [JsonProperty("phone")]
        public string Phone { get; set; } = "";
        [JsonProperty("homepage")]
        public string HomePage { get; set; } = "";
        [JsonProperty("nickname")]
        public string NickName { get; set; } = "";
        [JsonProperty("mail_config")]
        public MailConfig? MailConfig { get; set; }
    }
}
