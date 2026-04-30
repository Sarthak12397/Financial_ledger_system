using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LedgerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ILedgerRepository, LedgerRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitofWorkRepository>();

builder.Services.AddScoped<JournalEntryService>();
builder.Services.AddScoped<BalanceService>();


builder.Services.AddControllers();

var app = builder.Build();


app.UseHttpsRedirection();



app.Run();

