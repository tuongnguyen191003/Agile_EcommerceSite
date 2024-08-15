using Agile_Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Agile_Ecommerce.Repository
{
	public class SeedData
	{
		public static void SeedingData(DataContext _context)
		{
			_context.Database.Migrate();
			if(!_context.Products.Any())
			{
				CategoryModel iPhone = new CategoryModel
				{
					Name = "iPhone",
					Slug = "iphone",
					Description = "iphone is Large Brand in the world",
					Status = 1
				};
				CategoryModel Galaxy = new CategoryModel
				{
					Name = "Galaxy S",
					Slug = "samsung",
					Description = "Samsung is Large Brand in the world",
					Status = 1
				};
				BrandModel apple = new BrandModel
				{
					Name = "Apple",
					Slug = "apple",
					Description = "Apple is Large Brand in the world",
					Status = 1
				};
				BrandModel samsung = new BrandModel
				{
					Name = "Samsung",
					Slug = "samsung",
					Description = "Samsung is Large Brand in the world",
					Status = 1
				};
				_context.Products.AddRange(
					new ProductModel
					{
						Name = "iPhone 14 Promax",
						Slug = "iphone-14-promax",
						Description = "iphone 14 pro max với hiệu năng mạnh mẽ vượt trội",
						Image = "iphone-14-promax.jpg",
						Category = iPhone, Brand = apple,
						Price = 1200,
						
					},
					new ProductModel
					{
						Name = "Samsung Galaxy S23 FE 5G 8GB 128GB",
						Slug = "samsung-galaxy-s23-fe",
						Description = "Samsung S23 FE sở hữu màn hình Dynamic AMOLED 2X 6.4 inch, tần số quét 120Hz đi cùng chip Exynos 2200 8 nhân tạo độ mượt mà, thoải mái khi sử dụng. Bên cạnh đó, với mức pin 4.500 mAh, giúp người dùng có thể tha hồ đọc báo, lướt web cả ngày dài, kết hợp sạc nhanh 25W, tiết kiệm thời gian sạc. ",
						Image = "samsung-galaxy-s23fe.jpg",
						Category = Galaxy,
						Brand = samsung,
						Price = 1200,
                        
                    }
				);
				_context.SaveChanges();

			}
		}
	}
}
