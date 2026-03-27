using System.Net;
using Newtonsoft.Json;
using Feex.Application.Exceptions;
//using Feex.Application.Services;
using Feex.Infrastructure;
using Feex.Infrastructure.Repository;

namespace Feex.API.Middlewares
{
    public class GlobalErrorHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandler> _logger;
        public GlobalErrorHandler(RequestDelegate next, ILogger<GlobalErrorHandler> logger)
        {
            this._next = next;
            this._logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try 
            {
                await _next(context);
            } catch (Exception ex)
            {
                _logger.LogError($"An Error Occured \n Error: {ex.Message} \n InnerException: {ex.InnerException} \n StackTrace: \n {ex.StackTrace} ");
                await HandleExceptionAsync(context, ex); 
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception ex) 
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode httpStatusCode =HttpStatusCode.InternalServerError;
            context.Response.StatusCode =(int)httpStatusCode;
            var message = ex.Message;
            if(ex.InnerException?.Message != null)
            {
                message = $"{ex.Message} / {ex.InnerException.Message}";
            }

            var errorDetails = new ApiResponseDto<string>()
            {
                message = message,
                status = false
            };
            switch (ex) 
            {
                case InvalidException invalidException:
                    httpStatusCode = HttpStatusCode.UnprocessableEntity;
                    context.Response.StatusCode=(int)httpStatusCode;
                    errorDetails.status = false;
                    errorDetails.message = message;
                    break;
                case BadRequestException badRequestException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    context.Response.StatusCode =(int) httpStatusCode;
                    errorDetails.status = false;
                    errorDetails.message = message;
                    break;
                case DuplicateException duplicateException:
                    httpStatusCode = HttpStatusCode.Conflict;
                    context.Response.StatusCode = (int)httpStatusCode;
                    errorDetails.status = false;
                    errorDetails.message = message;
                    break;
                case NotFoundException notFoundException:
                    httpStatusCode = HttpStatusCode.NotFound;
                    context.Response.StatusCode = (int)httpStatusCode;
                    errorDetails.status = false;
                    errorDetails.message = message;
                    break;
                default:
                    break;
            }
            string response =JsonConvert.SerializeObject(errorDetails);
            context.Response.StatusCode = (int)httpStatusCode;
            return context.Response.WriteAsync(response);
        }
    }
}
