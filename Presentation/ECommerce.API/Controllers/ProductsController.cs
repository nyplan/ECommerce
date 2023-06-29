using ECommerce.Application.Abstractions.Storage;
using ECommerce.Application.Features.Commands.CreateProduct;
using ECommerce.Application.Features.Queries.GetAllProducts;
using ECommerce.Application.Repositories;
using ECommerce.Application.ViewModels.Products;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductReadRepository _productRead;
        private readonly IProductWriteRepository _productWrite;
        private readonly IFileWriteRepository _fileWrite;
        private readonly IFileReadRepository _fileRead;
        private readonly IProductImageFileReadRepository _productImageRead;
        private readonly IProductImageFileWriteRepository _productImageWrite;
        private readonly IInvoiceFileReadRepository _invoiceFileRead;
        private readonly IInvoiceFileWriteRepository _invoiceFileWrite;
        readonly IStorageService _storageService;
        readonly IConfiguration _configuration;


        readonly IMediator _mediator;


        public ProductsController(IProductReadRepository productRead, IProductWriteRepository productWrite, IFileWriteRepository fileWrite, IFileReadRepository fileRead, IProductImageFileReadRepository productImageRead, IProductImageFileWriteRepository productImageWrite, IInvoiceFileReadRepository invoiceFileRead, IInvoiceFileWriteRepository invoiceFileWrite, IStorageService storageService, IConfiguration configuration, IMediator mediator)
        {
            _productRead = productRead;
            _productWrite = productWrite;
            _fileWrite = fileWrite;
            _fileRead = fileRead;
            _productImageRead = productImageRead;
            _productImageWrite = productImageWrite;
            _invoiceFileRead = invoiceFileRead;
            _invoiceFileWrite = invoiceFileWrite;
            _storageService = storageService;
            _configuration = configuration;
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetAllProductQueryRequest request)
        {
            GetAllProductQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpGet("id")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            return Ok(await _productRead.GetByIdAsync(id, false));
        }
        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest request)
        {
            CreateProductCommandResponse response = await _mediator.Send(request);
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
        public async Task<IActionResult> Upload(string id)
        {
            List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("photo-images", Request.Form.Files);

            Product product = await _productRead.GetByIdAsync(id);

            await _productImageWrite.AddRangeAsync(result.Select(r => new ProductImageFile()
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<Product>() { product }
            }).ToList());

            await _productImageWrite.SaveAsync();

            return Ok();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProductImages(Guid id)
        {
            Product? product = await _productRead.Table.Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
            return Ok(product.Images.Select(p => new
            {
                Path = $"{_configuration["BaseStorageUrl"]}/{p.Path}",
                p.FileName,
                p.Id
            }));
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteProductImage(Guid id, Guid imageId)
        {
            Product? product = await _productRead.Table.Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
            ProductImageFile? imageFile =  product?.Images.FirstOrDefault(p => p.Id == imageId);
            product?.Images.Remove(imageFile);
            await _productWrite.SaveAsync();
            return Ok();
        }
    }
}
