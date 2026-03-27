using Hangfire;
using Microsoft.Extensions.Configuration;
using Feex.Application;
//using Feex.Application.Services;
//using Feex.Hangfire;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        o.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
    });// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireContext"));
});
//builder.Services.AddHangfireServer();
//builder.RegisterCustomServices(builder.Configuration);
//builder.Services.AddScoped<JobService>();
//builder.Services.AddScoped<PaystackProcessorService>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    //var hangfireJobService = services.GetRequiredService<JobService>();
    //hangfireJobService.ScheduleRecurringJobs();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard("/hangfire");
app.UseHangfireServer();

app.MapGet("/swagger/index.html", () => Results.Redirect("/hangfire"));
app.MapGet("/swagger", () => Results.Redirect("/hangfire"));
app.MapGet("/", () => Results.Redirect("/hangfire"));

app.Run();
