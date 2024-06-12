using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.SupplierRepo;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlowerManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierRepo repo = new SupplierRepo();

        // GET: api/Suppliers
        [HttpGet]
        public async Task<ActionResult<IList<Supplier>>> GetSuppliers()
        {
            var items = repo.GetSuppliers();
            return Ok(items);
        }
    }
}
