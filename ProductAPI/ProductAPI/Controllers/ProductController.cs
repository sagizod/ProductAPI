using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;

namespace ProductAPI.Controllers
{
    [Route("/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductContext _context;

        public ProductController(ProductContext context)
        {
            _context = context;
        }

        // GET: v1/Product/5
        /// <summary>
        /// Finds a product with a matching id
        /// </summary>
        /// <param name="id">Product id to find details for</param>
        /// <returns>Product with matching id, if present</returns>
        /// <response code="200">Product with ID found</response>
        /// <response code="404">Product with ID not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: v1/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updates a product with a matching id
        /// </summary>
        /// <param name="id">Id of the product to be updated</param>
        /// <param name="newproduct"></param>
        /// <returns></returns>
        /// <response code="200">Product updated successfully</response>
        /// <response code="404">Product with ID not found</response>
        /// <response code="409">Conflict on updating product</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PutsProduct(int id, [FromForm] Product newproduct)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Entry(product).State = EntityState.Modified;

            if (!string.IsNullOrWhiteSpace(newproduct.Price))
            {
                product.Price = newproduct.Price;
            }
            if (!string.IsNullOrWhiteSpace(newproduct.Name))
            {
                product.Name = newproduct.Name;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return Conflict();
                }
            }

            return Ok();
        }

        // POST: v1/Product
        /// <summary>
        /// Create a new product entry
        /// </summary>
        /// <param name="product">Product details, do not send Id data</param>
        /// <returns></returns>
        /// <response code="200">Product created successfully</response>
        /// <response code="400">Product data incompatible</response>
        /// <response code="409">Error creating product</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostProduct([FromForm] Product product)
        {
            _context.Products.Add(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict();
            }
            catch (DbUpdateException)
            {
                return Conflict();
            }

            CreatedAtActionResult result = CreatedAtAction("GetProduct", new { id = product.Id }, product);

            return result.StatusCode == StatusCodes.Status201Created ? Ok() : result;
        }

        // DELETE: v1/Product/5
        /// <summary>
        /// Deletes a product with a matching id
        /// </summary>
        /// <param name="id">Id of the product to be deleted</param>
        /// <returns></returns>
        /// <response code="200">Product with ID deleted</response>
        /// <response code="404">Product with ID not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
