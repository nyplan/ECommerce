using ECommerce.Application.Repositories;
using ECommerce.Application.RequestParameters;
using ECommerce.Application.Services;
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
        private readonly IWebHostEnvironment _environment;
        private readonly IFileService _fileService
        public ProductsController(IProductReadRepository productRead, IProductWriteRepository productWrite,
            IWebHostEnvironment environment, IFileService fileService)
        {
            _productRead = productRead;
            _productWrite = productWrite;
            _environment = environment;
            _fileService = fileService;
        }
        [HttpGet]
        public IActionResult Get([FromQuery] Pagination pagination)
        {
            var totalCount = _productRead.GetAll(false).Count();
            var products = _productRead.GetAll(false).Skip(pagination.Page * pagination.Size).Take(pagination.Size).Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate
            });
            return Ok(new
            {
                totalCount,
                products
            });
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
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _productWrite.RemoveAsync(id);
            await _productWrite.SaveAsync();
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload()
        {
            await _fileService.UploadAsync("resource/product-images", Request.Form.Files);
            return Ok();
        }

    }
}
