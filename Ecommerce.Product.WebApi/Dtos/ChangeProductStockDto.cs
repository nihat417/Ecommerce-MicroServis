namespace Ecommerce.Products.WebApi.Dtos
{
    public sealed record ChangeProductStockDto(Guid ProducId,int Quantity);
}
