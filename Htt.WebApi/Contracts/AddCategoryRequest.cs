using System.ComponentModel.DataAnnotations;

namespace Htt.WebApi.Contracts
{
	public class AddCategoryRequest
	{
		[Required(AllowEmptyStrings =false)]
		public string Name { get; set; } = string.Empty;
	}
}
