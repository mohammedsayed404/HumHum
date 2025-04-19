using Domain.Common;

namespace Domain.Entities;

public class Product : EntityBase<int>
{

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public string PublicImageId { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;


    public int CategoryId { get; set; }
    public ProductCategory Category { get; set; } = null!;


    //add rate prop
    public float Rate { get; set; }

}
