using EMI.Application.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EMI.Infrastructure.Services
{
    //Debby Branch
    public class ZohoEmailSender : IEmailSenderService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        private readonly ZohoTokenService _tokenService;

        public ZohoEmailSender(HttpClient httpClient, ZohoTokenService tokenService, IConfiguration config)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
            _config = config;
        }
        //Rename Branch
        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                // Get a fresh access token automatically
                var accessToken = await _tokenService.GetAccessTokenAsync();

                var payload = new
                {
                    fromAddress = _config["ZohoEmail:FromAddress"],
                    toAddress = to,
                    subject = subject,
                    content = body,
                    mailFormat = "html"
                };

                var json = JsonSerializer.Serialize(payload);
                var contentData = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Zoho-oauthtoken", accessToken);

                var url = $"{_config["ZohoEmail:ApiDomain"]}/api/accounts/{_config["ZohoEmail:AccountId"]}/messages";

                var response = await _httpClient.PostAsync(url, contentData);

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Zoho Response: " + responseContent);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending Zoho email: {ex.Message}");
                return false;
            }
        }
    }
}