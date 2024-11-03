using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using VideoGameStore.Models;
using SendGrid;

public class Program // Ensure this is the only Program class in your solution
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<IVideoGameStoreContext>(provider =>
        {
            var connectionString = builder.Configuration.GetConnectionString("Default");
            return new VideoGameStoreContext(connectionString);
        });
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
            // Configure production settings if necessary
        }

        app.UseStaticFiles();
        app.UseRouting();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}