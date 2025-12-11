using E_Commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly ApplicationDbContext _db;
        public OrdersController(ApplicationDbContext db) => _db = db;
        [HttpGet]
        public async Task<IActionResult> Get(int? customerId, DateTime? fromDate, DateTime? toDate)
        {
            var query = _db.Orders
                           .Include(o => o.OrderItems)
                           .AsQueryable();

            if (customerId.HasValue)
                query = query.Where(o => o.CustomerId == customerId.Value);
            if (fromDate.HasValue)
                query = query.Where(o => o.OrderDate >= fromDate.Value);
            if (toDate.HasValue)
                query = query.Where(o => o.OrderDate <= toDate.Value);

            var orders = await query.ToListAsync();

            // Map to DTOs
            var result = orders.Select(o => new
            {
                o.Id,
                o.CustomerId,
                o.OrderDate,
                o.TotalAmount,
                OrderItems = o.OrderItems.Select(oi => new
                {
                    oi.Id,
                    oi.ProductId,
                    oi.Quantity,
                    oi.UnitPrice
                }).ToList()
            });

            return Ok(result);
        }

      
        // GET /api/orders/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(int id)
        //{
        //    var order = await _db.Orders
        //                         .Include(o => o.OrderItems)
        //                         .ThenInclude(oi => oi.Product)
        //                         .Include(o => o.Customer)
        //                         .FirstOrDefaultAsync(o => o.Id == id);

        //    return order == null ? NotFound() : Ok(order);
        //}

       


        //[HttpPost]
        //public async Task<IActionResult> PostOrder([FromBody] Order order)
        //{
        //    if (order == null || order.OrderItems == null || !order.OrderItems.Any())
        //        return BadRequest("Order or order items are empty.");

        //    // Validate customer
        //    var customer = await _db.Customers.FindAsync(order.CustomerId);
        //    if (customer == null)
        //        return NotFound("Customer not found.");

        //    decimal totalAmount = 0;

        //    foreach (var item in order.OrderItems)
        //    {
        //        var product = await _db.Products.FindAsync(item.ProductId);
        //        if (product == null)
        //            return NotFound($"Product with Id {item.ProductId} not found.");

        //        // Assign unit price from product
        //        item.UnitPrice = product.Price;

        //        // Add to total amount
        //        totalAmount += product.Price * item.Quantity;

        //        // Clear navigation properties so EF doesn’t try to attach them
        //    }

        //    order.TotalAmount = totalAmount;
        //    //order.OrderDate = DateTime.UtcNow;

        //    _db.Orders.Add(order);
        //    await _db.SaveChangesAsync();

        //    return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        //}

        public class CreateOrderDto
        {
            public int CustomerId { get; set; }
            public List<CreateOrderItemDto> OrderItems { get; set; } = new();
        }

        public class CreateOrderItemDto
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] CreateOrderDto orderDto)
        {
            if (orderDto == null || orderDto.OrderItems == null || !orderDto.OrderItems.Any())
                return BadRequest("Order or order items are empty.");

            var customer = await _db.Customers.FindAsync(orderDto.CustomerId);
            if (customer == null)
                return NotFound("Customer not found.");

            var order = new Order
            {
                CustomerId = orderDto.CustomerId,
                OrderDate = DateTime.UtcNow
            };

            decimal totalAmount = 0;

            foreach (var itemDto in orderDto.OrderItems)
            {
                var product = await _db.Products.FindAsync(itemDto.ProductId);
                if (product == null)
                    return NotFound($"Product with Id {itemDto.ProductId} not found.");

                var orderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price
                };

                totalAmount += product.Price * itemDto.Quantity;

                order.OrderItems.Add(orderItem);
            }

            order.TotalAmount = totalAmount;

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            //return CreatedAtAction(nameof(Get), new { id = order.Id }, order
            //
            return Ok(new
            {
                success = true,
                message = "Order created successfully",
                //orderId = order.Id
            });

        }




    }

}
