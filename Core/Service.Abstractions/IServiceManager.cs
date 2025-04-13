namespace Service.Abstractions;

public interface IServiceManager
{
    public IPhotoService PhotoService { get; }
    public IProductService ProductService { get; }

    public ICartService CartService { get; }
    public IOrderService OrderService { get; }

}
