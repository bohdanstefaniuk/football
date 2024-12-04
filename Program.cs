using Football.Data;
using Football.ExternalServices;
using Football.Jobs;
using Football.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddResponseCompression();
builder.Services.AddWebOptimizer(pipeline =>
{
    pipeline.MinifyJsFiles("js/*.js");
    pipeline.MinifyCssFiles("css/*.css");
    pipeline.AddCssBundle("/css/app.css", "css/*.css");
    pipeline.AddJavaScriptBundle("/js/app.js", "js/*.js");
});

builder.Services.AddExternalServices(builder.Configuration);
builder.Services.AddBusinessServices();
builder.Services.AddJobs();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseResponseCompression();
app.UseWebOptimizer();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToPage("/Upcoming");

app.Run();