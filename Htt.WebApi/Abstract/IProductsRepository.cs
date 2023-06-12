using Htt.WebApi.Models;

namespace Htt.WebApi.Abstract
{
	public interface IProductsRepository
	{
		Task<List<Product>> GetAllProductsAsync();
		Task<Product> GetProductByIdAsync(int id);

		Task DeleteProductByIdAsync(int id);
		Task<Product> UpdateProductAsync(Product Product);
		Task<Product> AddNewProductAsync(Product Product);

		Task AddCategoryToProductAsync(int productId, int categoryId);
		Task DeleteCategoryFromProductAsync(int productId, int categoryId);
	}
}
