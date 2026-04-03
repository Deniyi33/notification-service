using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Feex.Application;
//using Feex.Application.Services;
//using Feex.Hangfire;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add Hangfire
builder.Services.AddHangfire(config =>
     config.UseSqlServerStorage(
         builder.Configuration.GetConnectionString("DefaultConnection")
         )
);

//Add Hangfire server
builder.Services.AddHangfireServer();

builder.Services.AddControllers();

var app = builder.Build();

//Enable Hangfire Dashboard
app.UseHangfireDashboard();

app.MapControllers();
app.Run();