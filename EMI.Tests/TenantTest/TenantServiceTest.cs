//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using EMI.Application.DTO;
//using EMI.Application.DTO.RequestDTO;
//using EMI.Application.Exceptions;
//using EMI.Application.Services;
//using EMI.Domain.Entities;
//using EMI.Infrastructure.Repository;
//using EMI.Infrastructure.UnitOfWork;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Moq;
//using Xunit;

//namespace Feex.Tests.TenantTest
//{
//    public class TenantServiceTest
//    {
//        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
//        private readonly Mock<IGenericRepository<Tenant>> _mockTenantRepo;
//        private readonly Mock<IWebHostEnvironment> _mockEnv;
//        private readonly Mock<ILogger<TenantService>> _mockLogger;
//        private readonly TenantService _service;
//        private readonly string tenantName = "System";

//        public TenantServiceTest()
//        {
//            _mockUnitOfWork = new Mock<IUnitOfWork>();
//            _mockTenantRepo = new Mock<IGenericRepository<Tenant>>();
//            _mockEnv = new Mock<IWebHostEnvironment>();
//            _mockLogger = new Mock<ILogger<TenantService>>();

//            _mockUnitOfWork.Setup(u => u.Tenant).Returns(_mockTenantRepo.Object);
//            _mockEnv.Setup(x => x.EnvironmentName).Returns("Development");

//            _service = new TenantService(_mockEnv.Object, _mockUnitOfWork.Object);

//        }

//        [Fact]
//        public async Task CreateTenant_ShouldSucceed_WhenValidRequestIsProvided()
//        {
//            // Arrange
//            var request = new TenantRequestDto
//            {
//                Name = "TestTenant",
//                Email = "test@example.com",
//                Phone = "1234567890",
//                Address = "Lagos",
//                CallBackUrl = "https://test.com"
//            };

//            _mockTenantRepo.Setup(r => r.InsertAsync(It.IsAny<Tenant>(), true))
//                .ReturnsAsync((Tenant t, bool _) => t);

//            // Act
//            var result = await _service.CreateTenantAsync(request,tenantName);

//            // Assert
//            Assert.True(result.status);
//            Assert.Equal("Created Successfully", result.message);
//            Assert.NotNull(result.data);
//            Assert.Equal("TestTenant", result.data.Name);
//        }

//        [Fact]
//        public async Task CreateTenant_ShouldFail_WhenRequiredFieldsAreMissing()
//        {
//            // Arrange
//            var request = new TenantRequestDto();

//            // Act
//            var result = await _service.CreateTenantAsync(request, tenantName);
//            //Assert

//            Assert.False(result.status);
//            Assert.Null(result.data);
//        }


//        [Fact]
//        public async Task CreateTenant_ShouldThrow_WhenTenantWithSameNameAlreadyExists()
//        {
//            // Arrange
//            var request = new TenantRequestDto
//            {
//                Name = "ExistingTenant",
//                Email = "existing@example.com",
//                Phone = "0000000000",
//                Address = "Test",
//                CallBackUrl = "https://test.com"
//            };

//            _mockTenantRepo.Setup(r => r.GetOne(It.IsAny<Func<Tenant, bool>>()))
//                .Returns(new Tenant { Name = "ExistingTenant" });

//            // Act 
//            var result = await _service.CreateTenantAsync(request, tenantName);
//            //Assert

//            Assert.False(result.status);
//            Assert.Null(result.data);
//        }
//    }

//}
