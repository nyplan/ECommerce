using Microsoft.AspNetCore.Identity;

namespace ECommerce.Domain.Entities.Identity
{
    public class User : IdentityUser<string> 
    {
        public string NameSurname { get; set; }
    }
}
