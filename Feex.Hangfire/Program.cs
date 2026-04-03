using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Feex.Application;
//using Feex.Application.Services;
//using Feex.Hangfire;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using EMI.Application.Interface;

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


using (var scope = app.Services.CreateScope() )
{
    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    RecurringJob.AddOrUpdate<IEmailService>(
    "send-recurring-email",
    x => x.SendEmailAsync(new EMI.Application.DTO.RequestDTO.SendEmailRequestDto
    {
        To = "mail@gmail.com",
        Subject = "scheduled email",
        Body = "it is sent every minute"
    }),
    "******" // every minute
   );
}

app.Run();