using E_Commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {


        private readonly ApplicationDbContext _db;
        public CustomersController(ApplicationDbContext db) => _db = db;

        // GET /api/customers?search=Rahul
        [HttpGet]
        public async Task<IActionResult> Get(string? search)
        {
            var query = _db.Customers.AsQueryable();

            // Search by Name or Email
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c =>
                    c.Name.Contains(search) || c.Email.Contains(search));
            }

            var customers = await query.ToListAsync();
            return Ok(customers);
        }

        // GET /api/customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            return customer == null ? NotFound() : Ok(customer);
        }

        // POST /api/customers
        [HttpPost]
        public async Task<IActionResult> Post(Customer customer)
        {
            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
        }

        // PUT /api/customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Customer customer)
        {
            if (id != customer.Id)
                return BadRequest();

            _db.Entry(customer).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE /api/customers/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var customer = await _db.Customers.FindAsync(id);
        //    if (customer == null)
        //        return NotFound(new { success = false, message = "Customer not found" });

        //    _db.Customers.Remove(customer);
        //    await _db.SaveChangesAsync();

        //    return Ok(new
        //    {
        //        success = true,
        //        message = "Customer deleted successfully",
        //        customerId = id
        //    });
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer == null)
                return NotFound(new { success = false, message = "Customer not found" });

            // Check if customer has any orders
            var hasOrders = await _db.Orders.AnyAsync(o => o.CustomerId == id);
            if (hasOrders)
                return BadRequest(new { success = false, message = "Cannot delete customer. They have orders." });

            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Customer deleted successfully",
                customerId = id
            });
        }


    }
}
