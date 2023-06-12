using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Htt.WebApi.Contracts
{
	public class AddProductRequest
	{
		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; } = string.Empty;

		[Required]
		[Range(0, double.MaxValue, ErrorMessage ="Price value must be greater than zero")]
		[Column(TypeName = "decimal(10, 2)")]
		public decimal Price { get; set; }
	}
}
