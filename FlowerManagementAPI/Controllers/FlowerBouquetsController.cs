using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Repository.FlowerBouquetRepo;

namespace FlowerManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowerBouquetsController : ControllerBase
    {
        private readonly IFlowerRepo repo = new FlowerRepo();

        // GET: api/FlowerBouquets
        [HttpGet]
        public async Task<ActionResult<IList<FlowerBouquet>>> GetFlowerBouquets()
        {
            IList<FlowerBouquet> flowerBouquets = repo.GetFlowers();
            return Ok(flowerBouquets);
        }

        // GET: api/FlowerBouquets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FlowerBouquet>> GetFlowerBouquet(int id)
        {
            var flowerBouquet = repo.GetFlower(id);

            if (flowerBouquet == null)
            {
                return NotFound();
            }

            return flowerBouquet;
        }

        // PUT: api/FlowerBouquets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlowerBouquet(int id, FlowerBouquet flowerBouquet)
        {
            if (id != flowerBouquet.FlowerBouquetId)
            {
                return BadRequest();
            }
            try
            {
                repo.Update(flowerBouquet);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlowerBouquetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FlowerBouquets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FlowerBouquet>> PostFlowerBouquet(FlowerBouquet flowerBouquet)
        {
            try
            {
                repo.Save(flowerBouquet);
                return Ok();
            }
            catch (DbUpdateException)
            {
                if (FlowerBouquetExists(flowerBouquet.FlowerBouquetId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE: api/FlowerBouquets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlowerBouquet(int id)
        {
            var flowerBouquet = repo.GetFlower(id);
            if (flowerBouquet == null)
            {
                return NotFound();
            }
            repo.Delete(flowerBouquet);
            return Ok();
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IList<FlowerBouquet>>> SearchByName(string name)
        {
            var items = repo.SearchByName(name);
            return Ok(items);
        }

        private bool FlowerBouquetExists(int id)
        {
            return repo.Exist(id);
        }
    }
}
