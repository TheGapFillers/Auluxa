using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auluxa.WebApp.Auth
{
    public class OAuthToken
    {
        public string token_type { get; set; }
        public string access_token { get; set; }
    }
}