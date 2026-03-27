using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using EMI.AInfrastructure.Services;
using EMI.Application.Interface;
//using EMI.Application.Services;
using EMI.Domain.Interfaces;
using EMI.Infrastructure.Repository;
using Feex.API;
using Feex.API.Middlewares;
using Feex.Application;
//using Feex.Application.Services;
using Feex.Infrastructure;
using Feex.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<EMIContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();






// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errorMessage = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .FirstOrDefault()?.ErrorMessage ?? "Validation failed";

        return new JsonResult(new ApiResponseDto<string> { status = false, message = errorMessage })
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    };
});

//builder.RegisterCustomServices(configuration);
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); 
        o.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All); 
    });


builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Auth Service",
        Description = "Auth Service API (Supports JWT + API Key)"
    });


    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using Bearer scheme.\n\nExample: `Bearer {token}`",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

  
    swagger.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key Authorization.\n\nExample: `ApiKey {secret}`",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKey"
    });

 
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        },
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Register memory cache
builder.Services.AddMemoryCache();

// Inject context accessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

//Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
var services = scope.ServiceProvider;
var context = services.GetRequiredService<EMIContext>();
//context.Database.Migrate();
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//HealthCheck Middleware
app.MapHealthChecks("/api/health");

app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseMiddleware<GlobalErrorHandler>();

app.UseAuthentication();

//app.UseMiddleware<AuthenticationMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();
