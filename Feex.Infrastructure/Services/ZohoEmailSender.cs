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
    public class ZohoEmailSender : IEmailSenderService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ZohoEmailSender(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                // Prepare the payload
                var payload = new
                {
                    fromAddress = _config["ZohoEmail:FromAddress"],
                    toAddress = to,
                    subject = subject,        // must be lowercase
                    content = body,
                    mailFormat = "html"       // Zoho supports "html" or "text"
                };

                var json = JsonSerializer.Serialize(payload);
                var contentData = new StringContent(json, Encoding.UTF8, "application/json");

                // Set Authorization header
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Zoho-oauthtoken", _config["ZohoEmail:ApiToken"]);

                // Build the full API URL
                var url = $"{_config["ZohoEmail:ApiDomain"]}/api/accounts/{_config["ZohoEmail:AccountId"]}/messages";

                // Send the POST request
                var response = await _httpClient.PostAsync(url, contentData);

                // Log full response for debugging
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Zoho Response: " + responseContent);

                // Return true only if Zoho responds with success
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