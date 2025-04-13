using Domain.Contracts;
using Domain.Entities.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using HumHum.Extensions;
using HumHum.Mock;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;
using Service.Abstractions;
using Services;
using Shared.Cloudinary;
using StackExchange.Redis;

namespace HumHum;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);



        #region DI Services

        builder.Services.AddControllersWithViews();


        builder.Services.AddDbContext<HumHumContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                        .AddEntityFrameworkStores<HumHumContext>();

        builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!)
           );

        builder.Services.Configure<CloudinarySettings>
            (builder.Configuration.GetSection(nameof(CloudinarySettings)));


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
        //builder.Services.AddFluentValidationAutoValidation(config =>
        //{
        //    config.DisableDataAnnotationsValidation = true;
        //});

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

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        #endregion

        app.Run();
    }
}
