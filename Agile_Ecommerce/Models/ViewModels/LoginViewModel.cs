using System.ComponentModel.DataAnnotations;

namespace Agile_Ecommerce.Models.ViewModels
{
	public class LoginViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập username")]
		public string Username { get; set; }
		
		[DataType(DataType.Password)/*, Required(ErrorMessage = "Yêu cầu nhập mật khẩu")*/]
		public string Password { get; set; }

		public string ReturnUrl { get; set; }
        public string VerificationCode { get; set; }
    }
}
