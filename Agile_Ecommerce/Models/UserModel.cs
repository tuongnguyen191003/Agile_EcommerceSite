using System.ComponentModel.DataAnnotations;

namespace ShoppingOnline.Models
{
	public class UserModel
	{
		public string Id { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập username")]
		public string Username { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập email"), EmailAddress]
		public string Email { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập sđt"), Phone]
		public string PhoneNumber { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage ="Yêu cầu nhập mật khẩu")]
		public string Password { get; set; }
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
        public string VerificationCode { get; set; }
    }
}
