﻿namespace LagoVista.UserAdmin
{
    public interface IOAuthSettings
    {
        string Redirect { get;  }
        OAuthConfig GitHubOAuth { get; }
        OAuthConfig LinkedInOAuth { get;  }
        OAuthConfig MicrosoftOAuth { get; }
        OAuthConfig GoogleOAuth { get;  }
    }

    public class OAuthConfig
    {
        public OAuthConfig(string clientId, string secret)
        {
            ClientId = clientId;
            Secret = secret;
        }

        public string ClientId { get;  }
        public string Secret { get; }   
    }
}
