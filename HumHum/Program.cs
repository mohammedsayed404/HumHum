using Domain.Contracts;
using FluentValidation;
using HumHum.Extensions;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;
using Service.Abstractions;
using Services;
using Shared.Cloudinary;

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


        builder.Services.Configure<CloudinarySettings>
            (builder.Configuration.GetSection(nameof(CloudinarySettings)));


        builder.Services.AddScoped<IServiceManager, ServiceManager>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddAutoMapper(typeof(Services.AssemblyReference).Assembly);



        builder.Services.AddScoped<IDbInitializer, DbInitializer>();


        builder.Services.AddValidatorsFromAssemblyContaining<Program>();


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
