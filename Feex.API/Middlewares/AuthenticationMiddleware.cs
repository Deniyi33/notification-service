using Feex.Core.Entities;
using Feex.Infrastructure.Repository;
using Feex.Core.Enums;
using Microsoft.Extensions.Logging;

namespace Feex.API.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDapperRepository _dapperRepository;
        private readonly ILoggerFactory _loggerfactory;

        public AuthenticationMiddleware(
            RequestDelegate next,
            IDapperRepository dapperRepository,
            ILoggerFactory loggerfactory)
        {
            _next = next;
            _dapperRepository = dapperRepository;
            _loggerfactory = loggerfactory;
        }

        public async Task Invoke(HttpContext context)
        {
            var _logger = _loggerfactory.CreateLogger<AuthenticationMiddleware>();
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (!string.IsNullOrEmpty(authHeader))
            {
                try
                {
                    if (authHeader.StartsWith("ApiKey ", StringComparison.OrdinalIgnoreCase))
                    {
                        var apiKey = authHeader.Split(" ").Last();

                        var query = "SELECT * FROM Tenants WHERE ApiKey = @key";
                        var tenant = await _dapperRepository.GetAsync<Tenant>(
                            query, new { key = apiKey }, DbConnectionSource.FEEX
                        );

                        if (tenant != null && !tenant.IsDeleted &&
                            tenant.Status.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase))
                        {
                            context.Items["Tenant"] = tenant;
                        }
                    }

                  
                    else if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in authentication middleware");
                }
            }

            await _next(context);
        }
    }

}
