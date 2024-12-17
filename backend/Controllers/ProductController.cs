using Microsoft.AspNetCore.Mvc;
using backend.Interfaces;
using backend.DTOs;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProduct()
    {
        var products = await _productRepository.GetAllProductAsync();

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] ProductCreate product)
    {
        if (product == null)
        {
            return BadRequest(new { message = "Os dados informados são inválidos." });
        }

        var result = await _productRepository.AddProductAsync(product);

        return CreatedAtAction(nameof(GetProductById), new { id = result }, new { id = result, message = "Produto criado com sucesso." });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdate product)
    {
        if (product == null)
        {
            return BadRequest(new { message = "Os dados informados são inválidos." });
        }

        var productExists = await _productRepository.GetProductByIdAsync(id);

        if (productExists == null)
        {
            return NotFound(new { message = "O produto informado não foi encontrado." });
        }

        await _productRepository.UpdateProductAsync(id, product);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var existingProduct = await _productRepository.GetProductByIdAsync(id);

        if (existingProduct == null)
        {
            return NotFound(new { message = "Produto não encontrado." });
        }

        await _productRepository.DeleteProductAsync(id);

        return NoContent();
    }
}
