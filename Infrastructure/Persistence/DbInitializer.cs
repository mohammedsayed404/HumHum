using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Persistence;

public class DbInitializer : IDbInitializer
{
    private readonly HumHumContext _dbContext;

    public DbInitializer(HumHumContext dbContext)
    {
        _dbContext = dbContext;
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



    }
}
