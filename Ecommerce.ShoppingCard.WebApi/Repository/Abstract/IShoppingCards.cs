using Ecommerce.ShoppingCard.WebApi.Dtos;
using Ecommerce.ShoppingCard.WebApi.Models;

namespace Ecommerce.ShoppingCard.WebApi.Repository.Abstract
{
    public interface IShoppingCards
    {

        #region get

        Task<OperationResult> GetAllShoppingCards(IConfiguration configuration,CancellationToken cancellation);

        #endregion

        #region post

        Task<OperationResult> CreateShoppingCards(CreateShoppingCardDto shoppingCardDto,CancellationToken cancellation);

        Task<OperationResult> CreateOrder(IConfiguration configuration,CancellationToken cancellation);

        #endregion
    }
}
