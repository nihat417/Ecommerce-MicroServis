﻿using Ecommerce.ShoppingCard.WebApi.Dtos;
using Ecommerce.ShoppingCard.WebApi.Repository.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.ShoppingCard.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCardController(IShoppingCards shoppingCards) : ControllerBase
    {

        [HttpGet("GetAllShoppingCards")]
        public async Task<IActionResult> GetAllShoppingCards(CancellationToken cancellationToken)
        {
            var shoppingCardss = await shoppingCards.GetAllShoppingCards(cancellationToken);
            if (shoppingCards == null) return NotFound();
            return Ok(shoppingCardss);
        }


        [HttpPost("CreateShoppingCard")]
        public async Task<IActionResult> CreateShopCards(CreateShoppingCardDto createShoppingCardDto,CancellationToken cancellationToken)
        {
            var result = await shoppingCards.CreateShoppingCards(createShoppingCardDto, cancellationToken);
            if(result.Success) return Ok(result);
            else return BadRequest(result);
        }
    }
}