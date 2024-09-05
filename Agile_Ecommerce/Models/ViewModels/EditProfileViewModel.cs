using ShoppingOnline.Repository.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agile_Ecommerce.Models.ViewModels
{
	public class EditProfileViewModel
	{
		public string FullName { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string? CurrentProfileImage { get; set; }
		[NotMapped]
		[FileExtension]
		public IFormFile? ProfileImageUpload { get; set; }
	}
}
