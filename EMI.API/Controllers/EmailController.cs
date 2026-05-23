using EMI.Application.DTO.RequestDTO;
using EMI.Application.Interface;
using Feex.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hangfire;
using EMI.AInfrastructure.Services;

namespace EMI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : BaseController
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailRequestDto request)
        {
            BackgroundJob.Enqueue<IEmailService>(x =>
                x.SendEmailAsync(request)
            );
        
            return Ok(new
            {
                status = true,
                message = "Email sent successfully"
            });
        }

    }
}
