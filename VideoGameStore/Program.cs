using VideoGameStore.Models;
using SendGrid;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<VideoGameStoreContext>(provider =>
    new VideoGameStoreContext(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddSingleton<ISendGridClient>(provider =>
{
    var apiKey = builder.Configuration.GetSection("SendGrid")["ApiKey"];
    return new SendGridClient(apiKey);
});

builder.Services.AddScoped<EmailService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
