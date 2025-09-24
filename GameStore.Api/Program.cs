using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");

// Register SQLite DbContext
builder.Services.AddSqlite<GameStoreContext>(connString);

// **Enable Razor views**
builder.Services.AddControllersWithViews();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:3000") // React dev server
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();
app.UseCors("AllowFrontend");

var logger = app.Logger; 

// Map Minimal API endpoints
app.MapGamesEndpoints();
app.MapAuthEndpoints();
// Map Razor Pages / MVC Controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Apply database migrations
app.MigrateDb();

logger.LogInformation("Using connection string: {ConnString}", connString);

app.Run();
