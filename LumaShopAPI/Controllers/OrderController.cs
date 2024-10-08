/*
 * <summary>
 * This controller handles API requests related to order management, including 
 * creating, updating, fetching, and retrieving all orders. It ensures proper 
 * authorization for each action based on user roles.
 * </summary>
 */


using LumaShopAPI.DTOModals.Common;
using LumaShopAPI.DTOModals.Order;
using LumaShopAPI.Entities;
using LumaShopAPI.LumaShopEnum;
using LumaShopAPI.Services;
using LumaShopAPI.Services.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace LumaShopAPI.Controllers
{
    [ApiController]
    [Route("order")]
    [Authorize]

    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly MongodbService _mongodbService;
        private readonly ProductService _productService;

        public OrderController(OrderService orderService, MongodbService mongodbService, ProductService productService)
        {
            _orderService = orderService;
            _mongodbService = mongodbService;
            _productService = productService;
        }

        [HttpPost]
        [Authorize(Roles = "CUSTOMER,CSR")]

       /*
        * Creates a new order based on the provided CreateOrderRequest. 
        * Validates stock availability before finalizing the order.
        */

        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var mongoClient = new MongoClient(_mongodbService.Database.Client.Settings);

            using var session = await mongoClient.StartSessionAsync();
            session.StartTransaction();

            try
            {
                var order = new Order
                {
                    CustomerId = request.CustomerId,
                    Items = request.Items.Select(i => new Item
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList(),
                    TotalAmount = request.TotalAmount,
                    Status = OrderStatusEnum.PENDING
                };

                var result = await _orderService.CreateOrderAsync(order);
                foreach (var item in request.Items)
                {
                    var isStockUpdated = await _productService.DeductStockAsync(item.ProductId, item.Quantity, session);
                    if (!isStockUpdated)
                    {
                        await session.AbortTransactionAsync();
                        return StatusCode(400, new APIResponse
                        {
                            Status = "error",
                            Message = $"Insufficient stock for product {item.ProductId}",
                            Data = null,
                            Errors = null
                        });
                    }
                }

                await session.CommitTransactionAsync();

                return CreatedAtAction(nameof(GetOrderById), new { id = result.Id }, new APIResponse
                {
                    Status = "success",
                    Message = "Order created successfully, stock updated",
                    Data = result,
                    Errors = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
                {
                    Status = "error",
                    Message = "An unexpected error occurred.",
                    Data = null,
                    Errors = new[] { ex.Message }
                });
            }
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "CUSTOMER,CSR,ADMIN")]

        /*
         * Updates the status of an existing order identified by its ID. 
         * Returns a 404 status if the order is not found.
         */

        public async Task<IActionResult> UpdateOrder(string id, [FromBody] UpdateOrderRequest request)
        {
            try
            {
                var existingOrder = await _orderService.GetOrderByIdAsync(id);
                if (existingOrder == null) return StatusCode(404, new APIResponse
                {
                    Status = "success",
                    Message = "Order not found!",
                    Data = null,
                    Errors = null
                });

                existingOrder.Status = request.Status;
                existingOrder.UpdatedAt = DateTime.UtcNow;
                var result = await _orderService.UpdateOrderAsync(id, existingOrder);
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Order updated successfully!",
                    Data = result,
                    Errors = null
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
                {
                    Status = "error",
                    Message = "An unexpected error occurred.",
                    Data = null,
                    Errors = new[] { ex.Message }
                });
            }

        }

        [HttpGet("{id}")]
        [Authorize(Roles = "CUSTOMER,CSR,ADMIN")]

        /*
         * Fetches an order based on the provided order ID. 
         * Returns a 404 status if the order is not found.
         */
        public async Task<IActionResult> GetOrderById(string id)
        {
            try
            {
                var result = await _orderService.GetOrderByIdAsync(id);
                if (result == null) return StatusCode(404, new APIResponse
                {
                    Status = "success",
                    Message = "Order not found!",
                    Data = null,
                    Errors = null
                });

                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Order fetch successfully!",
                    Data = result,
                    Errors = null
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
                {
                    Status = "error",
                    Message = "An unexpected error occurred.",
                    Data = null,
                    Errors = new[] { ex.Message }
                });
            }


        }

        [HttpGet]
        [Authorize(Roles = "CUSTOMER,CSR,ADMIN")]

        /*
         * Fetches all orders from the database.
         */
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var result = await _orderService.GetAllOrdersAsync();
                return StatusCode(200, new APIResponse
                {
                    Status = "success",
                    Message = "Orders fetch successfully!",
                    Data = result,
                    Errors = null
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
                {
                    Status = "error",
                    Message = "An unexpected error occurred.",
                    Data = null,
                    Errors = new[] { ex.Message }
                });
            }

        }
    }
}
