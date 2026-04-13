//using DinkToPdf;
//using DinkToPdf.Contracts;
//using EMI.Application.DTO;
//using EMI.Application.Interface;
//using EMI.Application.Services;
//using EMI.Application.Validators;
//using EMI.Domain.Entities;
//using EMI.Infrastructure;
//using EMI.Infrastructure.DbContexts;
//using EMI.Infrastructure.HttpClientService;
//using EMI.Infrastructure.Repository;
//using EMI.Infrastructure.UnitOfWork;
//using FluentValidation;
//using Mapster;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

//namespace EMI.Application
//{
//    public static class ServiceRegistration
//    {
//        public static WebApplicationBuilder RegisterCustomServices(this WebApplicationBuilder builder, IConfiguration Configuration)
//        {
//            #region Database related registration
//            // Register MSSQL context
//            builder.Services.AddDbContext<EMIContext>(options =>
//            {
//                options.UseSqlServer(Configuration.GetConnectionString("Ploutous"),
//                sqlServerOptionsAction: sqlOptions =>
//                {
//                    sqlOptions.CommandTimeout(1000);
//                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
//                });
//            },ServiceLifetime.Scoped);

//            // Register MongoDB context
//            builder.Services.AddSingleton(sp =>
//            {
//                var connectionString = Configuration.GetConnectionString("MongoContext");
//                var databaseName = Configuration.GetSection("ConnectionStrings").GetSection("MongoDbSettings").GetSection("DatabaseName").Value;
//                return new MongoDbContext(connectionString, databaseName);
//            });
//            //registered for 
//            builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

//            // Register mongoDB repository
//            builder.Services.AddScoped(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>));

//            // Register DapperContext
//            builder.Services.AddSingleton(sp =>
//            {
//                var connectionString = Configuration.GetConnectionString("Ploutous");
//                var hookMasterConnectionString = Configuration.GetConnectionString("Ploutous");
//                var nx360ConnectionString = Configuration.GetConnectionString("Ploutous");

//                return new DapperContext(connectionString, hookMasterConnectionString, nx360ConnectionString);
//            });
            
//            // Register the generic DapperRepository
//            builder.Services.AddTransient(typeof(IDapperRepository), typeof(DapperRepository));
//            #endregion
//            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//            //Let's maintain order of A-Z so it's easier finding things around
//            builder.Services.AddMemoryCache();            
//            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));            
          
//            builder.Services.AddScoped<ITenantService, TenantService>();
//            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//            builder.Services.AddValidatorsFromAssemblyContaining<DivisionRequestDtoValidator>();
          
//            builder.Services.AddHttpClient("ExternalApi", client =>
//            {
//                client.Timeout = TimeSpan.FromSeconds(30);
//            });

//            builder.Services.AddScoped<IHttpApiClientFactory, HttpApiClientFactory>();


//            builder.Services.AddDistributedMemoryCache();

//            return builder;

//        }
//    }
//}
