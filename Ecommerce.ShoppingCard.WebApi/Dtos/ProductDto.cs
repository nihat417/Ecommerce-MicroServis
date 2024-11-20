namespace Ecommerce.ShoppingCard.WebApi.Dtos
{
    public sealed record ProductDto(Guid Id, string Name, decimal Price, int Stock);
}
