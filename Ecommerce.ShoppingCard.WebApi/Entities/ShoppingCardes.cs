namespace Ecommerce.ShoppingCard.WebApi.Entities
{
    public sealed class ShoppingCardes
    {
        public ShoppingCardes()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Stock { get; set; }
    }
}
