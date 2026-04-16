using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// -------------------- LOGGING --------------------
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// -------------------- CONTROLLERS --------------------
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        o.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
    });

builder.Services.AddEndpointsApiExplorer();

// -------------------- HANGFIRE --------------------
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(
        builder.Configuration.GetConnectionString("HangfireConnection")
    );
});

builder.Services.AddHangfireServer(); // ?? REQUIRED

var app = builder.Build();

// -------------------- PIPELINE --------------------

// Dashboard FIRST
app.UseHangfireDashboard("/hangfire");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

// -------------------- TEST JOB --------------------
RecurringJob.AddOrUpdate(
    "hangfire-test-job",
    () => Console.WriteLine("?? Hangfire is working"),
    "* * * * *"
);

app.Run();