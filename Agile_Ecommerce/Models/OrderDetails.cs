using System.ComponentModel.DataAnnotations.Schema;

namespace Agile_Ecommerce.Models
{
	public class OrderDetails
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string OrderCode { get; set; }
		public int ProductId { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
        public int OrderModelId { get; set; }
        [ForeignKey("OrderModelId")]
        public OrderModel Order { get; set; }
        [ForeignKey("ProductId")]
		public ProductModel Product { get; set; }
	}
}
