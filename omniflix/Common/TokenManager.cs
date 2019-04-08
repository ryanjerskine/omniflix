using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace omniflix.Common
{
    public class TokenManager
    {
        private readonly IConfiguration _Configuration;
        private string _GoogleDriveToken { get; set; } = null;
        private DateTime _GoogleDriveTokenDate { get; set; } = DateTime.MinValue;

        public readonly string OmdbApiKey;
        public readonly string TmdbApiKey;

        public TokenManager(IConfiguration configuration)
        {
            this._Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.OmdbApiKey = this._Configuration.GetValue<string>("OmdbApiKey");
            this.OmdbApiKey = this._Configuration.GetValue<string>("TMDbApiKey");
        }

        public async Task<string> GetGoogleDriveTokenAsync()
        {
            if (this._GoogleDriveToken == null || this._GoogleDriveTokenDate < DateTime.UtcNow.AddHours(-4))
            {
                var token = await GoogleCredential
                    .FromJson(_Configuration.GetValue<string>("GoogleDriveSA")) // Loads key file  
                    .CreateScoped(new string[1] { "https://www.googleapis.com/auth/drive" }) // Gathers scopes requested  
                    .UnderlyingCredential // Gets the credentials  
                    .GetAccessTokenForRequestAsync(); // Gets the Access Token
                this._GoogleDriveToken = token;
                this._GoogleDriveTokenDate = DateTime.UtcNow.AddMinutes(-5);
            }
            
            return this._GoogleDriveToken;
        }
        public async Task<AuthenticationHeaderValue> GetGoogleDriveAuthHeaderAsync()
        {
            return new AuthenticationHeaderValue("Bearer", await this.GetGoogleDriveTokenAsync());
        }
    }
}