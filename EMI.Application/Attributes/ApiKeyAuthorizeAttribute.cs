//using EMI.Domain.Entities;
//using EMI.Infrastructure;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EMI.Application.Attributes
//{
//    public class ApiKeyAuthorizeAttribute : Attribute, IAuthorizationFilter
//    {
//        public void OnAuthorization(AuthorizationFilterContext context)
//        {
//            var tenant = context.HttpContext.Items["Tenant"] as Tenant;

//            if (tenant == null)
//            {
//                context.Result = new JsonResult(new ApiResponseDto<string>
//                {
//                    status = false,
//                    message = "Invalid or missing API Key"
//                })
//                {
//                    StatusCode = StatusCodes.Status401Unauthorized
//                };
//            }
//        }
//    }
//}
