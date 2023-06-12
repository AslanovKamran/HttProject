using Htt.WebApi.Abstract;
using Htt.WebApi.Contracts;
using Htt.WebApi.Models;
using Htt.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Htt.WebApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class ProductsController : ControllerBase
	{
		private readonly IProductsRepository _productRepository;
		private readonly ICategoriesRepository _categoriesRepository;

		public ProductsController(IProductsRepository productRepository, ICategoriesRepository categoriesRepository)
		{
			_productRepository = productRepository;
			_categoriesRepository = categoriesRepository;
		}

		[HttpGet]
		[ProducesResponseType(200)]
		[ProducesResponseType(204)]
		public async Task<ActionResult<List<ProductViewModel>>> GetAll()
		{
			var products = await _productRepository.GetAllProductsAsync();
			if (products == null || products.Count == 0) return NoContent();

			var result = new List<ProductViewModel>();
			foreach (var product in products)
			{
				var productViewModel = new ProductViewModel();
				productViewModel.Product = product;
				productViewModel.Categories = await _categoriesRepository.GetProductCategoriesByIdAsync(product.Id);
				result.Add(productViewModel);
			}

			return Ok(result);
		}

		[HttpGet]
		[Route("{id}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(204)]
		public async Task<ActionResult<ProductViewModel>> GetById(int id)
		{
			var product = await _productRepository.GetProductByIdAsync(id);
			if (product is null) return NoContent();

			var result = new ProductViewModel();
			result.Product = product;
			result.Categories = await _categoriesRepository.GetProductCategoriesByIdAsync(product.Id);

			return result;


		}

		[HttpPost]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<Product>> Add([FromForm] AddProductRequest request)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			Product product = new() { Name = request.Name, Price = request.Price };
			product = await _productRepository.AddNewProductAsync(product);
			return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
		}

		[HttpPut]
		[ProducesResponseType(201)]
		[ProducesResponseType(404)]
		public async Task<ActionResult<Product>> Update([FromForm] Product product)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			try
			{
				var updatedProduct = await _productRepository.UpdateProductAsync(product);
				return CreatedAtAction(nameof(GetById), new { id = updatedProduct.Id }, updatedProduct);

			}
			catch (Exception ex)
			{

				return NotFound("Product not found: " + ex.Message);
			}
		}

		[HttpDelete]
		[Route("{id}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		public async Task<ActionResult> Delete(int id)
		{
			var product = await _productRepository.GetProductByIdAsync(id);
			if (product is null) return NotFound();

			await _productRepository.DeleteProductByIdAsync(id);
			return NoContent();
		}

		[HttpPost]
		[Route("addCategory")]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<ProductViewModel>> AddCategory([FromForm]ProductCategoriesRequest request) 
		{
			var product = await _productRepository.GetProductByIdAsync(request.ProductId);
			if (product is null) return BadRequest("No such product");

			await	_productRepository.AddCategoryToProductAsync(request.ProductId, request.CategoryId);
			var productVm = await this.GetById(request.ProductId);
			return CreatedAtAction(nameof(GetById), new { id = request.ProductId }, productVm.Value);
		}

		[HttpDelete]
		[Route("deleteCategory")]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<ProductViewModel>> DeleteCategory([FromForm] ProductCategoriesRequest request)
		{
			var product = await _productRepository.GetProductByIdAsync(request.ProductId);
			if (product is null) return BadRequest("No such product");

			await _productRepository.DeleteCategoryFromProductAsync(request.ProductId, request.CategoryId);
			var productVm = await this.GetById(request.ProductId);
			return CreatedAtAction(nameof(GetById), new { id = request.ProductId }, productVm.Value);
		}
	}
}
