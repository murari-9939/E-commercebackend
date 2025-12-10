using E_Commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {


        private readonly ApplicationDbContext _db;
        public ProductsController(ApplicationDbContext db) => _db = db;

        // GET /api/products?search=phone&category=Mobile&page=1&pageSize=5

        [HttpGet]
        public async Task<IActionResult> Get(
            string? search,
            string? category,
            int page = 1,
            int pageSize = 10)
        {
            var query = _db.Products.AsQueryable();

            // Search by name
            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Name.Contains(search));

            // Filter by category
            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category);

            // Pagination
            var totalRecords = await query.CountAsync();
            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                TotalRecords = totalRecords,
                Page = page,
                PageSize = pageSize,
                Data = products
            });
        }

        // GET /api/products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _db.Products.FindAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        // POST /api/products
        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return Ok(new
            {
                success = true,
                message = "Product created successfully",
                productId = product.Id
            });
        }

        // PUT /api/products/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(int id, Product product)
        //{
        //    if (id != product.Id)
        //        return BadRequest();

        //    _db.Entry(product).State = EntityState.Modified;
        //    await _db.SaveChangesAsync();
        //    return NoContent();
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Product product)
        {
            var existingProduct = await _db.Products.FindAsync(id);
            if (existingProduct == null)
                return NotFound(new { message = "Product not found" });

            // Update fields
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.Category = product.Category;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Product updated successfully" });
        }


        // DELETE /api/products/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var product = await _db.Products.FindAsync(id);
        //    if (product == null)
        //        return NotFound(new { success = false, message = "Product not found" });

        //    _db.Products.Remove(product);
        //    await _db.SaveChangesAsync();

        //    return Ok(new
        //    {
        //        success = true,
        //        message = "Product deleted successfully",
        //        productId = id
        //    });
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
                return NotFound(new { success = false, message = "Product not found" });

            // Check if product is in any order
            var hasOrders = await _db.OrderItems.AnyAsync(oi => oi.ProductId == id);
            if (hasOrders)
                return BadRequest(new { success = false, message = "Cannot delete product. It is used in orders." });

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Product deleted successfully",
                productId = id
            });
        }

    }
}