using Bogus;
using Ecommerce.Products.WebApi.Context;
using Ecommerce.Products.WebApi.Dtos;
using Ecommerce.Products.WebApi.Entities;
using Ecommerce.Products.WebApi.Models;
using Ecommerce.Products.WebApi.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Products.WebApi.Repository.Concrete
{
    public class ProductRepository(ApplicationDbContext dbContext) : IProductRepository
    {
        #region get

        public async Task<OperationResult> GetAllProducts(CancellationToken cancellation)
        {
            try
            {
                var products = await dbContext.Products
                .OrderBy(p => p.Name)
                    .ToListAsync(cancellation);

                return new OperationResult
                {
                    Success = true,
                    Message = "Products retrieved successfully.",
                    ErrorMessage = null,
                    Data = products
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "An error occurred while retrieving products.",
                    ErrorMessage = ex.Message,
                    Data = null
                };
            }
        }

        #endregion

        #region post

        public async Task<OperationResult> CreateProduct(CreateProductDto createProduct, CancellationToken cancellationToken)
        {
            bool isNameExists = await dbContext.Products.AnyAsync(p => p.Name == createProduct.Name, cancellationToken);

            if (isNameExists)
                return new OperationResult
                {
                    Success = false,
                    Message = "Product name extist.",
                    ErrorMessage = null,
                };

            Product product = new()
            {
                Name = createProduct.Name,
                Price = createProduct.Price,
                Stock = createProduct.Stock,
            };
            await dbContext.Products.AddAsync(product, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new OperationResult { Success = true, Message = "successfully created", ErrorMessage = null, Data = product };
        }


        public async Task<OperationResult> AddFakeDatas()
        {
            var faker = new Faker();
            var products = new List<Product>();

            for (int i = 0; i < 100; i++)
            {
                var product = new Product
                {
                    Name = faker.Commerce.ProductName(),
                    Price = Convert.ToDecimal(faker.Commerce.Price()),
                    Stock = faker.Random.Int(1, 100)
                };
                products.Add(product);
            }

            await dbContext.Products.AddRangeAsync(products);
            await dbContext.SaveChangesAsync();

            return new OperationResult { Success = true, Message = "Successfully created 100 products", ErrorMessage = null };
        }

        public async Task<OperationResult> ChangeStockProduct(List<ChangeProductStockDto> productStockDtos, CancellationToken cancellationToken)
        {
            foreach (var item in productStockDtos)
            {
                Product? product = await dbContext.Products.FindAsync(item.ProducId, cancellationToken);
                if (product is not null)
                    product.Stock -= item.Quantity;
            }
            await dbContext.SaveChangesAsync();

            return new OperationResult { Success = true, Message = "successfully changed", ErrorMessage = null };
        }


        #endregion
    }
}
