using Microsoft.AspNetCore.Identity;
namespace Dashboard.Models.DTO
{
    public class CustomeIdentityUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ProfileImage { get; set; } = string.Empty;
    }
}
