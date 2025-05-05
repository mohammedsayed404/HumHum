using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Aggregates;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Persistence;

public class DbInitializer : IDbInitializer
{
    private readonly HumHumContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DbInitializer(HumHumContext dbContext, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitializeAsync()
    {

        if (_dbContext.Database.GetPendingMigrations().Any())
            await _dbContext.Database.MigrateAsync();


        if (!_dbContext.Restaurants.Any())
        {
            var json = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Data Seeding\restaurants.json");


            var restaurants = JsonSerializer.Deserialize<List<Restaurant>>(json);



            if (restaurants?.Any() == true)
                _dbContext.Restaurants.AddRange(restaurants);

            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }


        }


        if (!_dbContext.ProductCategories.Any())
        {
            var json = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Data Seeding\categories.json");


            var categories = JsonSerializer.Deserialize<List<ProductCategory>>(json);



            if (categories?.Any() == true)
                _dbContext.ProductCategories.AddRange(categories);

            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }


        }


        if (!_dbContext.Products.Any())
        {
            var json = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Data Seeding\products.json");


            var products = JsonSerializer.Deserialize<List<Product>>(json);



            if (products?.Any() == true)
                _dbContext.Products.AddRange(products);

            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }


        }


        if (!_dbContext.DeliveryMethods.Any())
        {
            var json = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Data Seeding\delivery.json");


            var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(json);



            if (deliveryMethods?.Any() == true)
                _dbContext.DeliveryMethods.AddRange(deliveryMethods);

            try
            {

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }


        }


    }

    public async Task InitializeIdentityAsync()
    {
        if (!_roleManager.Roles.Any())
        {
            await _roleManager.CreateAsync(new IdentityRole(Roles.Administrator));
            await _roleManager.CreateAsync(new IdentityRole(Roles.Customer));
            await _roleManager.CreateAsync(new IdentityRole(Roles.RestaurantManager));
        }



        if (!_userManager.Users.Any())
        {
            var Administrator = new ApplicationUser
            {
                DisplayName = nameof(Roles.Administrator),
                Email = "admin@gmail.com",
                UserName = "mmossyed525",
                PhoneNumber = "01061212129",
                Address = new Address
                {
                    City = "Cairo",
                    Street = "123 str",
                    Country = "Egypt",
                    FirstName = "Mohamed",
                    LastName = "Khafaga",
                }
            };

            var customer = new ApplicationUser
            {
                DisplayName = "Mohamed Khafaga",
                Email = "customer@gmail.com",
                UserName = "Mohamed.Sayed",
                PhoneNumber = "01061212126",
                Address = new Address
                {
                    City = "Cairo",
                    Street = "123 str",
                    Country = "Egypt",
                    FirstName = "Mohamed",
                    LastName = "Khafaga",
                }
            };

            var restaurantManager = new ApplicationUser
            {
                DisplayName = "abdellateef eid",
                Email = "RestaurantManager@gmail.com",
                UserName = "abdellateef",
                PhoneNumber = "01090116894",
                Address = new Address
                {
                    City = "Cairo",
                    Street = "123 str",
                    Country = "Egypt",
                    FirstName = "abdeallateef",
                    LastName = "eid",
                }
            };

            await _userManager.CreateAsync(Administrator, "Pa$$w0rd");
            await _userManager.CreateAsync(customer, "Pa$$w0rd");
            await _userManager.CreateAsync(restaurantManager, "Pa$$w0rd");

            #region test db
            //var res = await _userManager.CreateAsync(Administrator, "Pa$$w0rd");
            //var res02 = await _userManager.CreateAsync(customer, "Pa$$w0rd");

            //if (res.Succeeded) Console.WriteLine("admin done");

            //if (res02.Succeeded) Console.WriteLine("customer done");
            //else
            //{

            //    foreach (var err in res02.Errors)
            //    {
            //        Console.WriteLine(err.Description);
            //    }



            //    Console.WriteLine("error");
            //}
            #endregion


            await _userManager.AddToRoleAsync(Administrator, Roles.Administrator);
            await _userManager.AddToRoleAsync(customer, Roles.Customer);
            await _userManager.AddToRoleAsync(restaurantManager, Roles.RestaurantManager);


        }



    }
}
