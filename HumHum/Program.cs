using Domain.Contracts;
using Domain.Entities.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using HumHum.Extensions;
using HumHum.SMTP;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;
using Presentation.Mock;
using Service.Abstractions;
using Services;
using Shared.Cloudinary;
using Shared.Stripe;
using StackExchange.Redis;

namespace HumHum;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);



        #region DI Services

        builder.Services.AddControllersWithViews()
            .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);


        builder.Services.AddDbContext<HumHumContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        /*
        Step0 : Install ASP.NET Core Identity packages.
        Step1:
            Call services.AddIdentity<IdentityUser, IdentityRole>() to register Identity services.
            Connect Identity to a database using AddEntityFrameworkStores<ApplicationDbContext>().
        */
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                        .AddEntityFrameworkStores<HumHumContext>()
                        .AddDefaultTokenProviders();
        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Password settings
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 1; // You can change this length
            options.Password.RequireNonAlphanumeric = false; // No special characters
            options.Password.RequireUppercase = false; // Require capital letter
            options.Password.RequireLowercase = true; // Require small letter
        });


        // Configure Authentication Cookie
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.ExpireTimeSpan = TimeSpan.FromDays(14);
            options.SlidingExpiration = true;
        });
        builder.Services.AddControllersWithViews().AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
        builder.Services.AddTransient<IEmailSender, EmailSender>();




        builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!)
           );


        // In Program.cs, add these services before AddAuthentication
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>,
            UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>();


        // Fix authentication configuration in Program.cs
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddFacebook(facebookOptions =>
        {
            facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
            facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
        })
        .AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
            googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        });




        // Add to Program.cs for detailed logging
        builder.Services.AddLogging(logging =>
        {
            logging.AddConsole();
            logging.AddDebug();
            logging.SetMinimumLevel(LogLevel.Trace);
        });




        builder.Services.AddSession();

        builder.Services.Configure<CloudinarySettings>
            (builder.Configuration.GetSection(nameof(CloudinarySettings)));


        builder.Services.Configure<StripeSettings>
          (builder.Configuration.GetSection(nameof(StripeSettings)));



        builder.Services.AddScoped<ICartRepository, CartRepository>();
        builder.Services.AddScoped<IServiceManager, ServiceManager>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddAutoMapper(typeof(Services.AssemblyReference).Assembly);



        builder.Services.AddScoped<IDbInitializer, DbInitializer>();



        #region Tesing Current User
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<MockCurrentUser>();
        #endregion


        #region New Way Of Fluent Validation 
        builder.Services.AddFluentValidationAutoValidation();
        /*
        builder.Services.AddFluentValidationAutoValidation(config =>
        {
            config.DisableDataAnnotationsValidation = true;
        });
        */

        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        #endregion




        #endregion


        var app = builder.Build();

        await app.SeedDbAsync();


        #region Kesteral
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseSession();

        app.UseRouting();


        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        #endregion

        app.Run();
    }
}
