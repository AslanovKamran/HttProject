using Htt.WebApi.Models;

namespace Htt.WebApi.ViewModels
{
	public class ProductViewModel
	{
		public Product Product { get; set; } = new();
		public List<Category> Categories{ get; set; }= new();
	}
}
