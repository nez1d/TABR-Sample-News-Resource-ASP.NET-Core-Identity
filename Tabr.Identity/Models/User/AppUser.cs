using Microsoft.AspNetCore.Identity;

namespace Tabr.Identity.Entities.User
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }   
        public string LastName { get; set; }
    }
}
