using Microsoft.AspNetCore.Identity;

namespace Agile_Ecommerce.Models
{
	public class AppUserModel : IdentityUser
	{
		public string RoleId { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? ProfileImage { get; set; } // Path to the profile image
        public string VerificationCode { get; set; }

    }
}
