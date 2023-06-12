using Dapper;
using System.Data.SqlClient;
using System.Data;
using Htt.WebApi.Abstract;
using Htt.WebApi.Models;

namespace Htt.WebApi.Repository
{
	public class ProductsRepository : IProductsRepository
	{
		private readonly string _connectionString;
		public ProductsRepository(string connectionString) => _connectionString = connectionString;

		

		//Add a new Product and return the added Entity
		public async Task<Product> AddNewProductAsync(Product Product)
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				var parameters = new DynamicParameters();
				parameters.Add("Name", Product.Name, DbType.String, ParameterDirection.Input);
				parameters.Add("Price", Product.Price, DbType.Decimal, ParameterDirection.Input);

				string query = @"exec AddNewProduct @Name, @Price";
				var insertedProduct = await db.QuerySingleAsync<Product>(query, parameters);
				return insertedProduct;

			}
		}

		

		//Delete an existing Product
		public async Task DeleteProductByIdAsync(int id)
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				var parameters = new DynamicParameters();
				parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

				string query = @"exec DeleteProduct @Id";
				await db.ExecuteAsync(query, parameters);
			}
		}

		//Retrieve all Products
		public async Task<List<Product>> GetAllProductsAsync()
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				string query = @"exec GetAllProducts";
				return (await db.QueryAsync<Product>(query, null)).ToList();
			}


		}

		//Retrieve a specific Product
		public async Task<Product> GetProductByIdAsync(int id)
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				var parameters = new DynamicParameters();
				parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

				string query = @"exec GetProductById @Id";
				return await db.QueryFirstOrDefaultAsync<Product>(query, parameters);
			}
		}

		//Update a specific Product
		public async Task<Product> UpdateProductAsync(Product Product)
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				var parameters = new DynamicParameters();
				parameters.Add("Id", Product.Id, DbType.Int32, ParameterDirection.Input);
				parameters.Add("Name", Product.Name, DbType.String, ParameterDirection.Input);
				parameters.Add("Price", Product.Price, DbType.Decimal, ParameterDirection.Input);

				string query = @"exec UpdateProduct @Id, @Name, @Price";
				var updatedProduct = await db.QuerySingleAsync<Product>(query, parameters);
				return updatedProduct;
			}
		}

		public async Task AddCategoryToProductAsync(int productId, int categoryId)
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				var parameters = new DynamicParameters();
				parameters.Add("ProductId", productId, DbType.Int32, ParameterDirection.Input);
				parameters.Add("CategoryId", categoryId, DbType.Int32, ParameterDirection.Input);

				string query = @"exec AddCategoryToProduct @ProductId, @CategoryId";
				await db.ExecuteAsync(query, parameters);
			}
		}

		public async Task DeleteCategoryFromProductAsync(int productId, int categoryId)
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				var parameters = new DynamicParameters();
				parameters.Add("ProductId", productId, DbType.Int32, ParameterDirection.Input);
				parameters.Add("CategoryId", categoryId, DbType.Int32, ParameterDirection.Input);

				string query = @"exec DeleteCategoryFromProduct @ProductId, @CategoryId";
				await db.ExecuteAsync(query, parameters);
			}
		}
	}
}
