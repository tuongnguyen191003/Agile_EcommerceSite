namespace Agile_Ecommerce.Models
{
    public class RatingModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; } // Thay đổi thành string
        public int Rating { get; set; }
        public string Content { get; set; }
        public ProductModel Product { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
