using ECommerce.Application.Features.Commands.Product.CreateProduct;
using ECommerce.Application.Features.Commands.Product.DeleteProduct;
using ECommerce.Application.Features.Commands.Product.UpdateProduct;
using ECommerce.Application.Features.Commands.ProductImage.DeleteProductImage;
using ECommerce.Application.Features.Commands.ProductImage.UploadProductImage;
using ECommerce.Application.Features.Queries.Product.ById;
using ECommerce.Application.Features.Queries.Product.GetAllProducts;
using ECommerce.Application.Features.Queries.ProductImage.ProductImages;
using ECommerce.Infrastructure.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] ProductsQueryRequest request)
        {
            ProductsQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] ProductByIdQueryRequest request)
        {
            ProductByIdQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest request)
        {
            CreateProductCommandResponse response = await _mediator.Send(request);
            return StatusCode((int)HttpStatusCode.Created);
        }


        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteProductCommandRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest request)
        {
            request.Files = Request.Form.Files;
            await _mediator.Send(request);
            return Ok();
        }


        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProductImages([FromRoute] AllProductImagesQueryRequest request)
        {
            List<AllProductImagesQueryResponse> response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("[action]/{Id}")]
        public async Task<IActionResult> DeleteProductImage([FromRoute] DeleteProductImageCommandRequest request, [FromQuery] Guid imageId)
        {
            request.ImageId = imageId;
            await _mediator.Send(request);
            return Ok();
        }
    }
}
