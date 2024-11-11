namespace Ecommerce.Products.WebApi.Services
{
    public class Responses
    {
        public record class GeneralResponse(bool Flag, string Message);
        public record class LoginResponse(bool Flag, string AccsesToken, string RefreshToken, string Message);
    }
}
