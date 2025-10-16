var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

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