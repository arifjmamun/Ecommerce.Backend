using System;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Backend.API.Helpers;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ecommerce.Backend.API.Controllers
{
  [SwaggerTag("Carts")]
  [Produces("application/json")]
  [Route("api/carts")]
  [ApiController]
  public class CartsController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly ICartService _cartService;
    public CartsController(ICartService cartService, IMapper mapper)
    {
      _mapper = mapper;
      _cartService = cartService;
    }

    /// <summary>
    /// Get Cart by ID
    /// </summary>
    [HttpGet("id")]
    public async Task<ActionResult<ApiResponse<Cart>>> Get([FromQuery] string cartId, [FromQuery] string customerId)
    {
      try
      {
        if (cartId.IsEmpty() && customerId.IsEmpty())
        {
          throw new Exception("Cart ID or Customer ID should be present in query params.");
        }
        var cart = await _cartService.GetCartById(cartId, customerId);
        return cart.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Add a new cart product
    /// </summary>
    [HttpPost("add/product")]
    public async Task<ActionResult<ApiResponse<Cart>>> AddProduct(AddCartProductDto dto)
    {
      try
      {
        var createdCart = await _cartService.AddCartProduct(dto);
        return createdCart.CreateSuccessResponse("Cart created successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Assign CustomerId to a cart
    /// </summary>
    /// <param name="cartId"></param>
    /// <param name="customerId"></param>
    [HttpPatch("{cartId}/assign/{customerId}")]
    public async Task<ActionResult<ApiResponse<Cart>>> AssignCustomer(string cartId, string customerId)
    {
      try
      {
        if (cartId.IsEmpty()) throw new Exception("Cart ID is empty.");
        if (customerId.IsEmpty()) throw new Exception("Customer ID is empty.");
        var updatedCart = await _cartService.AssignCustomerId(cartId, customerId);
        return updatedCart.CreateSuccessResponse();
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Update Cart product quantity
    /// </summary>
    /// <param name="cartId"></param>
    [HttpPatch("update/{cartId}/product-quantity")]
    public async Task<ActionResult<ApiResponse<Cart>>> UpdateProductQuantity(string cartId, UpdateCartProductDto dto)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(cartId)) return BadRequest("Cart ID is empty.");
        var updatedCart = await _cartService.UpdateProductQuantity(cartId, dto);
        return updatedCart.CreateSuccessResponse("Cart updated successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }

    /// <summary>
    /// Remove Cart product
    /// </summary>
    /// <param name="cartId"></param>
    [HttpDelete("remove/{cartId}/product/{cartProductId}")]
    public async Task<ActionResult<ApiResponse<Cart>>> RemoveProduct(string cartId, string cartProductId)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(cartId)) return BadRequest("Cart ID is empty.");
        if (string.IsNullOrWhiteSpace(cartProductId)) return BadRequest("Cart product ID is empty.");
        var updatedCart = await _cartService.RemoveCartProduct(cartId, cartProductId);
        return updatedCart.CreateSuccessResponse("Cart product removed successfully!");
      }
      catch (Exception exception)
      {
        return BadRequest(exception.CreateErrorResponse());
      }
    }
  }
}