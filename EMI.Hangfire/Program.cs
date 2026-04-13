using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using EMI.Application;
//using EMI.Application.Services;
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

RecurringJob.AddOrUpdate<IEmailService>(
  "loan-reminder",
  x => x.SendLoanReminder("mail@gmail.com"),
  Cron.Monthly // every minute
 );

Console.WriteLine("Recurring job registered");

app.MapControllers();

app.Run();