using Microsoft.AspNetCore.Mvc;
using UnitTestMVC.Models;
using UnitTestMVC.Repositories;

namespace UnitTestMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsAPIController : ControllerBase
    {
        private readonly IRepository<Product> _context;

        public ProductsAPIController(IRepository<Product> context)
        {
            _context = context;
        }

        // GET: api/ProductsAPI
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(await _context.GeAll());
        }

        // GET: api/ProductsAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/ProductsAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Update(product);

            return NoContent();
        }

        // POST: api/ProductsAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostProduct(Product product)
        {
            await _context.Create(product);

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/ProductsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            await _context.Delete(product);

            return NoContent();
        }

    }
}
