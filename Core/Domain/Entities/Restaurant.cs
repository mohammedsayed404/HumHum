using Domain.Common;

namespace Domain.Entities;

public class Restaurant : EntityBase<int>
{
    public string Name { get; set; } = string.Empty;


    public string PublicImageId { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = new HashSet<Product>();
}
