using ECommerce.Application.Repositories;
using Microsoft.AspNetCore.Mvc;

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

        
    }
}
