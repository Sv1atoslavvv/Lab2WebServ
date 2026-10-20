using EmailSenderWeb.Lifetimes;
using EmailSenderWeb.Services;
using Microsoft.Extensions.Options;

namespace EmailSenderWeb;

public sealed class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.Configure<EmailSenderOptions>(
            _configuration.GetSection(EmailSenderOptions.SectionName));

        // EmailSender is stateless, so Transient creates a fresh instance when requested.
        services.AddTransient<IEmailSender, EmailSender>();

        // Demonstration of DI lifetimes required by the assignment.
        services.AddTransient<TransientProbe>();
        services.AddScoped<ScopedProbe>();
        services.AddSingleton<SingletonProbe>();
    }

    public void Configure(WebApplication app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    }
}
