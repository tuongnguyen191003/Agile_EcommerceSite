using Microsoft.AspNetCore.Identity;

namespace Agile_Ecommerce.Models
{
	public class AppUserModel : IdentityUser
	{
		public string RoleId { get; set; }
        public string VerificationCode { get; set; }

    }
}
