namespace Agile_Ecommerce.Models
{
	public class WishListItems
	{
		public int Id { get; set; } // Thêm Id để phân biệt từng mục
		public string UserId { get; set; } // Liên kết với người dùng
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public decimal Price { get; set; }
		public string Image { get; set; }

	}
}
