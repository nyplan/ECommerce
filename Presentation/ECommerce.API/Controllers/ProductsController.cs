using ECommerce.Application.Abstractions.Storage;
using ECommerce.Application.Repositories;
using ECommerce.Application.RequestParameters;
using ECommerce.Application.ViewModels.Products;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Repositories;
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
        private readonly IFileWriteRepository _fileWrite;
        private readonly IFileReadRepository _fileRead;
        private readonly IProductImageFileReadRepository _productImageRead;
        private readonly IProductImageFileWriteRepository _productImageWrite;
        private readonly IInvoiceFileReadRepository _invoiceFileRead;
        private readonly IInvoiceFileWriteRepository _invoiceFileWrite;
        readonly IStorageService _storageService;
        public ProductsController(IProductReadRepository productRead, IProductWriteRepository productWrite, IFileWriteRepository fileWrite, IFileReadRepository fileRead, IProductImageFileReadRepository productImageRead, IProductImageFileWriteRepository productImageWrite, IInvoiceFileReadRepository invoiceFileRead, IInvoiceFileWriteRepository invoiceFileWrite, IStorageService storageService)
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
            var datas = await _storageService.UploadAsync("files", Request.Form.Files);
            //var datas = await _fileService.UploadAsync("resource/files", Request.Form.Files);
            await _productImageWrite.AddRangeAsync(datas.Select(d => new ProductImageFile()
            {
                FileName = d.fileName,
                Path = d.pathOrContainerName,
                Storage = _storageService.StorageName
            }).ToList());
            await _productImageWrite.SaveAsync();

            //var datas = await _fileService.UploadAsync("resource/files", Request.Form.Files);
            //_productImageWrite.AddRangeAsync(datas.Select(d => new ProductImageFile()
            //{
            //    FileName = d.fileName,
            //    Path = d.path
            //}).ToList());
            //await _productImageWrite.SaveAsync();

            //_invoiceFileWrite.AddRangeAsync(datas.Select(d => new InvoiceFile()
            //{
            //    FileName = d.fileName,
            //    Path = d.path,
            //    Price = 20
            //}).ToList());
            //await _invoiceFileWrite.SaveAsync();

            //_fileWrite.AddRangeAsync(datas.Select(d => new Domain.Entities.File()
            //{
            //    FileName = d.fileName,
            //    Path = d.path
            //}).ToList());
            //await _fileWrite.SaveAsync();

            //var d1 = _fileRead.GetAll();
            //var d2 = _productImageRead.GetAll();
            //var d3 = _invoiceFileRead.GetAll();

            return Ok();
        }

    }
}
