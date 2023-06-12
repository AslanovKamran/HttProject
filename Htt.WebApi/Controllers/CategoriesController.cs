using System.Data;
using Htt.WebApi.Abstract;
using Htt.WebApi.Contracts;
using Htt.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Htt.WebApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]

	public class CategoriesController : ControllerBase
	{
		private readonly ICategoriesRepository _repos;

		public CategoriesController(ICategoriesRepository repos) => _repos = repos;

		[HttpGet]
		[ProducesResponseType(200)]
		[ProducesResponseType(204)]
		public async Task<ActionResult<List<Category>>> GetAll()
		{
			var result = await _repos.GetAllCategoriesAsync();
			return (result == null || result.Count == 0) ? NoContent() : Ok(result);
		}

		[HttpGet]
		[Route("{id}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(204)]
		public async Task<ActionResult<Category>> GetById(int id)
		{
			var result = await _repos.GetCategoryByIdAsync(id);
			return result == null ? NoContent() : Ok(result);
		}

		[HttpPost]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		public async Task<ActionResult<Category>> Add([FromForm] AddCategoryRequest request)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			Category category = new() { Name = request.Name };
			category = await _repos.AddNewCategoryAsync(category);
			return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
		}

		[HttpPut]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public async Task<ActionResult<Category>> Update([FromForm] Category category)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			try
			{
				var updatedCategory = await _repos.UpdateCategoryAsync(category);
				return CreatedAtAction(nameof(GetById), new { id = updatedCategory.Id }, updatedCategory);

			}
			catch (Exception ex)
			{

				return NotFound("Cateogry not found: " + ex.Message);
			}
		}

		[HttpDelete]
		[Route("{id}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		public async Task<ActionResult> Delete(int id)
		{
			var category = await _repos.GetCategoryByIdAsync(id);
			if (category is null) return NotFound();

			await _repos.DeleteCategoryByIdAsync(id);
			return NoContent();
		}

	}
}
