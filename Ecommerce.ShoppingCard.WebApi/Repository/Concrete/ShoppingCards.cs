using Ecommerce.ShoppingCard.WebApi.Context;
using Ecommerce.ShoppingCard.WebApi.Dtos;
using Ecommerce.ShoppingCard.WebApi.Entities;
using Ecommerce.ShoppingCard.WebApi.Models;
using Ecommerce.ShoppingCard.WebApi.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace Ecommerce.ShoppingCard.WebApi.Repository.Concrete
{
    public class ShoppingCards(ApplicationDbContext dbContext) : IShoppingCards
    {
        #region get

        public async Task<OperationResult> GetAllShoppingCards(IConfiguration configuration,CancellationToken cancellationToken)
        {
            List<ShoppingCardes> shoppingCards = await dbContext.shoppingCards.ToListAsync(cancellationToken);
            HttpClient httpclient = new HttpClient();

            string productsEndpoint = $"http://{configuration.GetSection("HttpRequest:Products").Value}/api/Product/GetAllProducts";
            var message = await httpclient.GetAsync(productsEndpoint);

            Result<List<ProductDto>> products = new();

            if (!message.IsSuccessStatusCode)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Failed to retrieve products from external service."
                };
            }

            if(message.IsSuccessStatusCode) 
                products = await message.Content.ReadFromJsonAsync<Result<List<ProductDto>>>(cancellationToken);

            List<ShoppingCardDto> response = shoppingCards.Select(s => new ShoppingCardDto()
            {
                Id = s.Id,
                ProductId = s.ProductId,
                Quantity = s.Quantity,
                ProductName = products!.Data!.First(p => p.Id == s.ProductId).Name,
                ProductPrice = products!.Data!.First(p => p.Id == s.ProductId).Price,
            }).ToList();

            if (products == null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Failed to parse products from external service."
                };
            }

            return new OperationResult
            {
                Success = true,
                Data = new
                {
                    Products = response
                }
            };
        }

        #endregion

        #region Post

        public async Task<OperationResult> CreateShoppingCards(CreateShoppingCardDto shoppingCardDto, CancellationToken cancellation)
        {
            ShoppingCardes shoppingCard = new()
            {
                ProductId = shoppingCardDto.ProductId,
                Quantity = shoppingCardDto.Quantity,
            };

            await dbContext.shoppingCards.AddAsync(shoppingCard, cancellation);
            await dbContext.SaveChangesAsync(cancellation);

            return new OperationResult()
            {
                Success = true,
                Data = shoppingCard,
                Message = "shopping card created"
            };
        }

        public async Task<OperationResult> CreateOrder(IConfiguration configuration, CancellationToken cancellationToken)
        {
            List<ShoppingCardes> shoppingCards = await dbContext.shoppingCards.ToListAsync(cancellationToken);
            HttpClient httpclient = new HttpClient();

            string productsEndpoint = $"http://{configuration.GetSection("HttpRequest:Products").Value}/api/Product/GetAllProducts";
            var message = await httpclient.GetAsync(productsEndpoint);

            Result<List<ProductDto>> products = new();

            if (!message.IsSuccessStatusCode)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Failed to retrieve products from external service."
                };
            }
            if(message.IsSuccessStatusCode) 
               products = await message.Content.ReadFromJsonAsync<Result<List<ProductDto>>>(cancellationToken);

            List<CreateOrderDto> response = shoppingCards.Select(s => new CreateOrderDto()
            {
                ProductId = s.ProductId,
                Quantity = s.Quantity,
                Price = products!.Data!.First(p => p.Id == s.ProductId).Price,
            }).ToList();

            string ordersEndpoint = $"http://{configuration.GetSection("HttpRequest:Orders").Value}/api/Order/createOrder";
            string stringjson = JsonSerializer.Serialize(response);

            var content = new StringContent(stringjson,Encoding.UTF8,"application/json"); 
            var orderMessage = await httpclient.PostAsync(ordersEndpoint, content);

            if(orderMessage.IsSuccessStatusCode)
            {
                dbContext.RemoveRange(shoppingCards);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            /*if (products == null)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Failed to parse products from external service."
                };
            }*/

            return new OperationResult
            {
                Success = true,
                Data = new
                {
                    Products = response
                },
                Message = "Order created"
            };
        }

        #endregion
    }
}
