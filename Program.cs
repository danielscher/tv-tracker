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
builder.Services.AddDbContext<TvTrackerContext>(options => 
    options
    .UseSqlite(sqliteSourceStr));

// inject services
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<UserMediaService>();
builder.Services.AddScoped(typeof(MediaService<>));
builder.Services.AddHttpClient<TmbdService>(
    client =>
    {   
        client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
    }
);

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

/*  Initialize DB with seed data.  */
using var scope = app.Services.CreateScope();
var ctx = scope.ServiceProvider.GetRequiredService<TvTrackerContext>();
ctx.Database.Migrate();
if (!ctx.Profiles.Any())
{
    TvTrackerContext.SeedData(ctx);
}
/**/

app.Run();
