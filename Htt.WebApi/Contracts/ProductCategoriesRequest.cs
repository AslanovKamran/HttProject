using System.ComponentModel.DataAnnotations;

namespace Htt.WebApi.Contracts
{
	public class ProductCategoriesRequest
	{
		[Required]
		public int ProductId { get; set; }
		[Required]
		public int CategoryId { get; set; }
	}
}
