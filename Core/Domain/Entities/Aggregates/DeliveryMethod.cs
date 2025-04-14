using Domain.Common;

namespace Domain.Entities.Aggregates;

public class DeliveryMethod : EntityBase<int>
{

    public string ShortName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DeliveryTime { get; set; } = string.Empty;
    public decimal Price { get; set; }


    public ICollection<Order> Orders { get; set; } = new HashSet<Order>();

}