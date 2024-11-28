namespace Ecommerce.ShoppingCard.WebApi.Dtos
{
    public sealed record ChangeProductStockDto(Guid ProductId,int Quantity);
}
