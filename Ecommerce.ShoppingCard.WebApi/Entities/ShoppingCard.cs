namespace Ecommerce.ShoppingCard.WebApi.Entities
{
    public sealed class ShoppingCard
    {
        public ShoppingCard()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Stock { get; set; }
    }
}
