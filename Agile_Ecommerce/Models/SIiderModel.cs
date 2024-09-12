using ShoppingOnline.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agile_Ecommerce.Models
{
	 // Thêm using này

	
	public class SliderModel
	{

		public int Count { get; set; }
		public int Id { get; set; }
		//[Required(ErrorMessage = "Yêu cầu không được bổ trống tên slider")]
		public string Name { get; set; }
		//[Required(ErrorMessage = "Yêu cầu không được bổ trống mô tả")]
		public string Description { get; set; }
		public int? Status { get; set; }
		public string Image { get; set; }

		[NotMapped]
		[FileExtension]
		public IFormFile
			ImageUpload
		{ get; set; }

	}
}
