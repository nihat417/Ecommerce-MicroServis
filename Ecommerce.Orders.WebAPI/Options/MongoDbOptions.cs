namespace Ecommerce.Orders.WebAPI.Options
{
    public sealed record MongoDbOptions
    {
        public string ConnectionString { get; set; } = default!;
        public string DatabaseName { get; set; } = default!;
    }
}
