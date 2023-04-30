using ECommerce.Application.Repositories;
using ECommerce.Application.ViewModels.Products;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductReadRepository _productRead;
        private readonly IProductWriteRepository _productWrite;
        public ProductsController(IProductReadRepository productRead, IProductWriteRepository productWrite)
        {
            _productRead = productRead;
            _productWrite = productWrite;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_productRead.GetAll(false));
        }
        [HttpGet("id")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            return Ok(await _productRead.GetByIdAsync(id, false));
        }
        [HttpPost]
        public async Task<IActionResult> Post(CreateProduct model)
        {
            await _productWrite.AddAsync(new()
            {
                Name = model.Name,
                Stock = model.Stock,
                Price = model.Price
            });
            await _productWrite.SaveAsync();
            return StatusCode((int)HttpStatusCode.Created);
        }
        [HttpPut]
        public async Task<IActionResult> Put(UpdateProduct model)
        {
            Product product = await _productRead.GetByIdAsync(model.Id);
            product.Name = model.Name;
            product.Stock = model.Stock;
            product.Price = model.Price;
            await _productWrite.SaveAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]string id)
        {
            await _productWrite.RemoveAsync(id);
            await _productWrite.SaveAsync();
            return Ok(); 
        }
        
    }
}
