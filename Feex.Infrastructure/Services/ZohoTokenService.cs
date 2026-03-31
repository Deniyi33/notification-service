using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace EMI.Infrastructure.Services
{
    public class ZohoTokenService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public ZohoTokenService(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var clientId = _config["ZohoEmail:ClientId"];
            var clientSecret = _config["ZohoEmail:ClientSecret"];
            var refreshToken = _config["ZohoEmail:RefreshToken"];

            // Build the URL for refresh token
            var url = $"https://accounts.zoho.com/oauth/v2/token?refresh_token={refreshToken}&client_id={clientId}&client_secret={clientSecret}&grant_type=refresh_token";

            var response = await _httpClient.PostAsync(url, null);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            // Parse JSON
            var jsonDoc = JsonDocument.Parse(content);
            var newAccessToken = jsonDoc.RootElement.GetProperty("access_token").GetString();

            return newAccessToken;
        }
    }
}