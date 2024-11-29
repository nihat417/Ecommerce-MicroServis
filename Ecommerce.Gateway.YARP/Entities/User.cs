namespace Ecommerce.Gateway.YARP.Entities
{
    public sealed class User
    {
        public User() =>Id = Guid.NewGuid();
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
