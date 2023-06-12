using System.Data;
using System.Data.SqlClient;
using Dapper;
using Htt.WebApi.Abstract;
using Htt.WebApi.Models;

namespace Htt.WebApi.Repository
{
	public class CategoriesRepository : ICategoriesRepository
	{
		private readonly string _connectionString;
		public CategoriesRepository(string connectionString) => _connectionString = connectionString;

		//Add a new Category and return the added Entity
		public async Task<Category> AddNewCategoryAsync(Category category)
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				var parameters = new DynamicParameters();
				parameters.Add("Name", category.Name, DbType.String, ParameterDirection.Input);

				string query = @"exec AddNewCategory @Name";
				var insertedCategory = await db.QuerySingleAsync<Category>(query, parameters);
				return insertedCategory;

			}
		}

		//Delete an existing Category
		public async Task DeleteCategoryByIdAsync(int id)
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				var parameters = new DynamicParameters();
				parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

				string query = @"exec DeleteCategory @Id";
				await db.ExecuteAsync(query, parameters);
			}
		}

		//Retrieve all Categories
		public async Task<List<Category>> GetAllCategoriesAsync()
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				string query = @"exec GetAllCategories";
				return (await db.QueryAsync<Category>(query, null)).ToList();
			}


		}

		//Retrieve a specific Category
		public async Task<Category> GetCategoryByIdAsync(int id)
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				var parameters = new DynamicParameters();
				parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

				string query = @"exec GetCategoryById @Id";
				return await db.QueryFirstOrDefaultAsync<Category>(query, parameters);
			}
		}

		//Update a specific Category
		public async Task<Category> UpdateCategoryAsync(Category category)
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				var parameters = new DynamicParameters();
				parameters.Add("Id", category.Id, DbType.Int32, ParameterDirection.Input);
				parameters.Add("Name", category.Name, DbType.String, ParameterDirection.Input);

				string query = @"exec UpdateCategory @Id, @Name";
				var updatedCategory = await db.QuerySingleAsync<Category>(query, parameters);
				return updatedCategory;
			}
		}

		//Retrieve a list of Categories related to a specific Product
		public async Task<List<Category>> GetProductCategoriesByIdAsync(int productId)
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				var parameters = new DynamicParameters();
				parameters.Add("ProductId", productId, DbType.Int32, ParameterDirection.Input);

				string query = @"exec GetAllCategoriesByProductId @ProductId";
				return (await db.QueryAsync<Category>(query, parameters)).ToList();
			}
		}
	}
}
