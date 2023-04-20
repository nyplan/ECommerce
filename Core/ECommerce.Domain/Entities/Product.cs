using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public long Stock { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
