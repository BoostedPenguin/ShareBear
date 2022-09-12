using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShareBear.Data.Models
{
    public class AuthenticateResponse
    {
        public string GlobalId { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public AuthenticateResponse(Users user, string jwtToken, string refreshToken)
        {
            GlobalId = user.UserGlobalIdentifier;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
