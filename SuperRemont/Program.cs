using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

var loggerFactory = LoggerFactory.Create(static loggingBuilder => { loggingBuilder.AddSerilog(); });

var logger = loggerFactory.CreateLogger<Program>();

logger.LogInformation("Starting web host");

var app = builder.Build();

// Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Serve static files from wwwroot
app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    // API routes first
    endpoints.MapControllerRoute(
                                 name: "api",
                                 pattern: "api/{controller}/{action=Index}/{id?}");

    // Fallback to React app for all other routes
    endpoints.MapFallbackToController("Index", "Home");
});

app.Run();