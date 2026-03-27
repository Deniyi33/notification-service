//using Microsoft.AspNetCore.Hosting;
//using Feex.Application.DTO;
//using Feex.Application.Helpers;
//using Feex.Application.Interface;
//using Feex.Core.Entities;
//using Feex.Core.Enums;
//using Feex.Infrastructure;
//using Feex.Infrastructure.UnitOfWork;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Feex.Application.Exceptions;
//using Feex.Application.DTO.RequestDTO;

//namespace Feex.Application.Services
//{
//    public class TenantService : ITenantService
//    {
//        private readonly IWebHostEnvironment _environment;
//        private readonly IUnitOfWork _unitOfWork;

//        public TenantService(IWebHostEnvironment environment, IUnitOfWork unitOfWork)
//        {
//            _environment = environment;
//            _unitOfWork = unitOfWork;
//        }
//        public async Task<ApiResponseDto<Tenant>> CreateTenantAsync(TenantRequestDto request, string tenantName)
//        {
//            var response = new ApiResponseDto<Tenant>();

//            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
//            {
//                response.status = false;
//                response.message = "Name and Email are required.";
//                return response;
//            }

//            var existingTenant = _unitOfWork.Tenant.GetOne(t => t.Name.ToLower() == request.Name.ToLower());
//            if (existingTenant != null)
//            {
//                response.status = false;
//                response.message = "Tenant with the same name already exists.";
//                return response;
//            }

//            var tenantToAdd = new Tenant
//            {
//                Name = request.Name,
//                Address = request.Address,
//                Email = request.Email,
//                CallBackUrl = request.CallBackUrl,
//                Phone = request.Phone,
//                Status = TenantStatus.ACTIVE.ToString(),
//                CreatedBy = tenantName,
//                ApiKey = GenericHelper.GenerateApiKey(_environment.EnvironmentName)
//            };

//            var tenantAdded = await _unitOfWork.Tenant.InsertAsync(tenantToAdd);

//            response.data = tenantAdded;
//            response.message = "Created Successfully";
//            response.status = true;

//            return response;
//        }

//        public async Task<ApiResponseDto<Tenant>> ReGenerateApiKey(Tenant tenant)
//        {
//            var response = new ApiResponseDto<Tenant>();

//            var ApiKey = GenericHelper.GenerateApiKey(_environment.EnvironmentName);
//            tenant.ApiKey = ApiKey;
//            var tenantUpdated = await _unitOfWork.Tenant.UpdateAsync(tenant);

//            response.data = tenantUpdated;
//            response.message = "Api Key regenerated Successfully";
//            response.status = true;
//            return response;
//        }

//        public async Task<ApiResponseDto<Tenant>> UpdateWebhookUrl(Tenant tenant, string callBackUrl)
//        {
//            var response = new ApiResponseDto<Tenant>();
//            tenant.CallBackUrl = callBackUrl;
//            var tenantUpdated = await _unitOfWork.Tenant.UpdateAsync(tenant);

//            response.data = tenantUpdated;
//            response.message = "Webhook Url updated Successfully";
//            response.status = true;
//            return response;
//        }
//    }
//}
