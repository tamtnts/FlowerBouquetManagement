using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.CategoryRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlowerManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepo repo = new CategoryRepo();

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IList<Category>>> GetCategories()
        {
            var items = repo.GetCategories();
            return Ok(items);
        }
    }
}
