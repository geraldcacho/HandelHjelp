using Microsoft.AspNetCore.Identity;

namespace HandleHjelp.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UsernameChangeLimit { get; set; } = 10;
        public byte[]? ProfilePicture { get; set; }
        public bool IsNotifyByEmail { get; set; }
        public bool IsNotifyByMobile { get; set; }

    }
}
