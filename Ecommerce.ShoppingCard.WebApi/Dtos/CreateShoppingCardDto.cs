namespace Ecommerce.ShoppingCard.WebApi.Dtos
{
    public sealed record CreateShoppingCardDto(Guid ProductId, int Quantity);
    public sealed record ShoppingCardDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
    }
}
