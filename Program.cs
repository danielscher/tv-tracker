using Microsoft.EntityFrameworkCore;
using TvTracker.Data;
using TvTracker.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// set up path to the project dir.
var dbPath = Path.Combine(AppContext.BaseDirectory, "TvTracker.db");
var sqliteSourceStr = $"Data Source={dbPath}";

// inject contexts
builder.Services.AddDbContext<ProfileContext>(options => options.UseSqlite(sqliteSourceStr));

// inject services
builder.Services.AddScoped<ProfileService>();

// builder.Logging.AddConsole();
// builder.Logging.SetMinimumLevel(LogLevel.Debug);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
