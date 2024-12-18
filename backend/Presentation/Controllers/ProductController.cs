﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Application.DTOs;
using Backend.Domain.Interfaces;

namespace Backend.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    /// <summary>
    /// Retrieve all products from the database.
    /// </summary>
    /// <returns>A list of all products.</returns>
    [HttpGet("GetAllProduct")]
    public async Task<IActionResult> GetAllProduct()
    {
        var products = await _productRepository.GetAllProductAsync();

        return Ok(products);
    }

    /// <summary>
    /// Retrieve a specific product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to retrieve.</param>
    /// <returns>The requested product if found, or a 404 status code if not found.</returns>
    [HttpGet("GetProductById/{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    /// <summary>
    /// Add a new product to the database.
    /// </summary>
    /// <param name="product">The product details to add.</param>
    /// <returns>The ID of the created product and a success message, or an error message if the operation fails.</returns>
    [HttpPost("AddProduct")]
    public async Task<IActionResult> AddProduct([FromBody] ProductCreate product)
    {
        if (product == null)
        {
            return BadRequest(new { message = "Os dados informados são inválidos." });
        }

        try
        {
            var result = await _productRepository.AddProductAsync(product);

            if (result == null || result <= 0)
            {
                return BadRequest(new { message = "Erro ao criar o produto." });
            }

            return CreatedAtAction(
                nameof(GetProductById),
                new { id = result },
                new { id = result, message = "Produto criado com sucesso." });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao adicionar produto: {ex.Message}");
            return StatusCode(500, new { message = "Ocorreu um erro no servidor." });
        }
    }

    /// <summary>
    /// Update an existing product in the database.
    /// </summary>
    /// <param name="id">The ID of the product to update.</param>
    /// <param name="product">The updated product details.</param>
    /// <returns>No content if the update is successful, or an error message if the operation fails.</returns>
    [HttpPut("UpdateProductById/{id}")]
    public async Task<IActionResult> UpdateProductById(int id, [FromBody] ProductUpdate product)
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

    /// <summary>
    /// Delete an existing product from the database.
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    /// <returns>No content if the deletion is successful, or an error message if the product is not found.</returns>
    [HttpDelete("DeleteProductById/{id}")]
    public async Task<IActionResult> DeleteProductById(int id)
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