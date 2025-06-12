using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Handlers.Commands.AddProductCommand;
using Product.Application.Handlers.Commands.DecrementProductStockCommand;
using Product.Application.Handlers.Commands.DeleteProductCommand;
using Product.Application.Handlers.Commands.EditProductCommand;
using Product.Application.Handlers.Commands.IncrementProductStockCommand;
using Product.Application.Handlers.Queries.FindProductByIdQuery;
using Product.Application.Handlers.Queries.FindProductsByNameQuery;
using Product.Application.Handlers.Queries.FindProductsByStockRangeQuery;
using Product.Application.Handlers.Queries.FindProductsQuery;

namespace Product.Controller.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> FindProducts()
    {
        var products = await mediator.Send(new FindProductsQuery());
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> FindProductById(int id)
    {
        var product = await mediator.Send(new FindProductByIdQuery(id));

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] AddProductCommandDto product)
    {
        var result = await mediator.Send(new AddProductCommand(product.ProductName, product.Stock));
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditProduct(int id, [FromBody] AddProductCommandDto product)
    {
        var result = await mediator.Send(new EditProductCommand(id, product.ProductName, product.Stock));

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await mediator.Send(new DeleteProductCommand(id));

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPut("{id}/add-to-stock/{quantity}")]
    public async Task<IActionResult> IncrementStock(int id, int quantity)
    {
        var product = await mediator.Send(new IncrementProductStockCommand(id, quantity));

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPut("{id}/decrement-stock/{quantity}")]
    public async Task<IActionResult> DecrementStock(int id, int quantity)
    {
        var product = await mediator.Send(new DecrementProductStockCommand(id, quantity));

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string name)
    {
        var products = await mediator.Send(new FindProductsByNameQuery(name));

        return Ok(products);
    }

    [HttpGet("stock-level")]
    public async Task<IActionResult> SearchByStockRange([FromQuery] int min, [FromQuery] int max)
    {
        var products = await mediator.Send(new FindProductsByStockRangeQuery(min, max));

        return Ok(products);
    }
}