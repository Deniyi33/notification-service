using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using EMI.Infrastructure;
using EMI.Domain.Entities;
using System.Security.Claims;
using Newtonsoft.Json;

namespace EMI.API.Middlewares
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public AuthorizeAttribute()
        {

        }


        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            var tenant = context.HttpContext.Items["Tenant"]! as Tenant;

            if (tenant == null)
            {
                context.Result = new JsonResult(new ApiResponseDto<string> { status = false, message = "Unauthorized!" })
                { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            //Set claims for tenentId, currenttenant
            var principal = context.HttpContext.User as ClaimsPrincipal;

            var customClaims = new List<Claim>()
            {
                new Claim("tenantId",tenant.Id.ToString()),
                new Claim("currenttenant", JsonConvert.SerializeObject(tenant))
            };

            var customIdentity = new ClaimsIdentity(customClaims,"Custom");

            principal.AddIdentity(customIdentity);

            context.HttpContext.User = principal;
        }
    }

}
