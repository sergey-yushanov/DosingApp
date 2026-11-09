using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DosingApp.Models.AgroAssist.Dto
{
    class LoginRequestDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }

    class LoginDto
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("access_token_expires_in")]
        public int AccessTokenExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("refresh_token_expires_in")]
        public int RefreshTokenExpiresIn { get; set; }

        [JsonPropertyName("superset_token")]
        public string SupersetToken { get; set; }

        [JsonPropertyName("superset_token_expires_in")]
        public int? SupersetTokenExpiresIn { get; set; }
    }
}
