using Ecommerce.ShoppingCard.WebApi.Context;
using Ecommerce.ShoppingCard.WebApi.Dtos;
using Ecommerce.ShoppingCard.WebApi.Entities;
using Ecommerce.ShoppingCard.WebApi.Models;
using Ecommerce.ShoppingCard.WebApi.Repository.Abstract;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.ShoppingCard.WebApi.Repository.Concrete
{
    public class ShoppingCards(ApplicationDbContext dbContext) : IShoppingCards
    {
        #region get

        public async Task<OperationResult> GetAllShoppingCards(CancellationToken cancellationToken)
        {
            List<ShoppingCardes> shoppingCards = await dbContext.shoppingCards.ToListAsync(cancellationToken);

            HttpClient client = new HttpClient();

            var message = await client.GetAsync("http://products:8080/api/Product/GetAllProducts");

            Result<List<ProductDto>> products = new();

            if (!message.IsSuccessStatusCode)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Failed to retrieve products from external service."
                };
            }

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

        async Task<OperationResult> IShoppingCards.CreateShoppingCards(CreateShoppingCardDto shoppingCardDto, CancellationToken cancellation)
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

        #endregion
    }
}
