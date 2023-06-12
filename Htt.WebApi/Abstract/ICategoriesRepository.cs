using Htt.WebApi.Models;

namespace Htt.WebApi.Abstract
{
	public interface ICategoriesRepository
	{
		Task<List<Category>> GetAllCategoriesAsync();
		Task<Category> GetCategoryByIdAsync(int id);

		Task DeleteCategoryByIdAsync(int id);
		Task<Category> UpdateCategoryAsync(Category category);
		Task<Category> AddNewCategoryAsync(Category category);

		Task<List<Category>> GetProductCategoriesByIdAsync(int productId);

	}
}
