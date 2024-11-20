namespace Ecommerce.ShoppingCard.WebApi.Models
{
    public sealed class Result<T>
    {
        public T? Data { get; set; } = default;
    }
}
