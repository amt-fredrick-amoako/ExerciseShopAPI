using System;
using System.Threading.Tasks;
using Exercise.Model;
using Exercise.Database;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Exercise.Controllers
{
    [Route("/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<string> errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(error => error.ErrorMessage)
                    .ToList();

                string message = string.Join("\n", errors);
                return BadRequest(message);

            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            // if (id != product.ID)
            //     return BadRequest();
            // _context.Entry(product).State = EntityState.Modified;

            // try
            // {
            //     await _context.SaveChangesAsync();
            // }
            // catch (DbUpdateConcurrencyException)
            // {
            //     if (!ProductExists(id))
            //     {
            //         return NotFound();
            //     }
            //     else
            //     {
            //         throw;
            //     }
            // }

            // return NoContent();
            // Validate the payload
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the product with the specified id exists
            var existingProduct = _context.Products.Find(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            // Update the product
            existingProduct.Name = product.Name;
            existingProduct.Weight = product.Weight;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete(("{id}"))]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(temp => temp.ID == id);
        }
    }
}