using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<ProductImageFile> Images { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; }
    }
}
