using CorrelationId;
using CorrelationId.DependencyInjection;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDefaultCorrelationId(options =>
{
    options.AddToLoggingScope = true;
    options.IncludeInResponse = true;
    options.EnforceHeader = false;
});

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithCorrelationId()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("logs/payment-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();



builder.Services.AddDbContext<LedgerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(
        builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddHangfireServer();
builder.Services.AddScoped<BalanceVerificationJob>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ILedgerRepository, LedgerRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitofWorkRepository>();

builder.Services.AddScoped<JournalEntryService>();
builder.Services.AddScoped<BalanceService>();


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
var app = builder.Build();
app.UseCorrelationId();

// Checkout part

app.MapControllers();
app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<BalanceVerificationJob>(
    "balance-verification",
    job => job.ExecuteAsync(),
    Cron.Hourly);
app.UseHttpsRedirection();

app.Run();
