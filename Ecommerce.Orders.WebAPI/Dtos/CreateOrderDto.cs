    namespace Ecommerce.Orders.WebAPI.Dtos
    {
        public record CreateOrderDto(Guid ProductId,int Quantity,decimal Price);
    }
