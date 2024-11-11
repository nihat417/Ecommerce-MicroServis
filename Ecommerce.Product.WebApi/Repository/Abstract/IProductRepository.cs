using Ecommerce.Products.WebApi.Dtos;
using Ecommerce.Products.WebApi.Models;

namespace Ecommerce.Products.WebApi.Repository.Abstract
{
    public interface IProductRepository
    {
        #region get

        Task<OperationResult> GetAllProducts(CancellationToken cancellation);

        #endregion

        #region post

        Task<OperationResult> CreateProduct(CreateProductDto productDto, CancellationToken cancellationToken);

        Task<OperationResult> AddFakeDatas();

        #endregion
    }
}
