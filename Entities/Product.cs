using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Entities
{
	public class Product
	{
		
		public int Id { get; set; }
		[Required(ErrorMessage= "The name field cannot be empty")]
		public string Name { get; set; }
		[Required(ErrorMessage = "The description field cannot be empty")]
		public string Description { get; set; }
		[Range(1,1000,ErrorMessage ="Price must be between 1-1000")]
		public double Price { get; set; }

		[Range(1, 100, ErrorMessage = "Discount must be between 1-100")]
		public double Discount { get; set; }
		public string ImagePath { get; set; }
	}
}
