using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Feex.Core.Entities;
using Feex.API.Middlewares;

namespace Feex.API.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        private StringValues tenantHeader;

        protected Tenant? CurrentTenant
        {
            get
            {
                var tenant = User.FindFirstValue("currenttenant");
                if (tenant != null)
                {
                    return JsonConvert.DeserializeObject<Tenant>(tenant);
                }
                return null;
            }
        }
        protected long TenantId
        {
            get
            {
                var tenantId = User.FindFirstValue("tenantId");
                if (tenantId != null)
                {
                    return long.Parse(tenantId);
                }
                return default;
            }

        }
    }
}
