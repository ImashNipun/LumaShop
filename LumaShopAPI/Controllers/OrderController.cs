using LumaShopAPI.DTOModals.Common;
using LumaShopAPI.DTOModals.Order;
using LumaShopAPI.Entities;
using LumaShopAPI.LumaShopEnum;
using LumaShopAPI.Services;
using LumaShopAPI.Services.Database;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace LumaShopAPI.Controllers
{
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
                        return BadRequest(new APIResponse
                        {
                            Status = "error",
                            Message = $"Insufficient stock for product {item.ProductId}",
                            Data = null,
                            Errors = null
                        });
                    }
                }

                // Step 3: Commit the transaction if everything is successful
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
