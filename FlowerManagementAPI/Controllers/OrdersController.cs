using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Repository.OrderRepo;

namespace FlowerManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepo repo = new OrderRepo();

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IList<Order>>> GetOrders()
        {
            IList<Order> orders = repo.GetOrders();
            return Ok(orders);
        }

        [HttpGet("Report")]
        public async Task<ActionResult<IList<Order>>> GetOrdersForReport(string startDateStr, string endDateStr)
        {
            DateTime startDate = DateTime.ParseExact(startDateStr, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(endDateStr, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            IList<Order> orders = repo.GetOrdersForReport(startDate, endDate);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            Order order = repo.GetOrder(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // GET: api/Orders/5
        [HttpGet("ByCustomer/{id}")]
        public async Task<ActionResult<IList<Order>>> GetOrderByCustomerId(int id)
        {
            var list = repo.GetOrderByCustomerId(id);

            if (list == null)
            {
                return NotFound();
            }

            return Ok(list);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }
            try
            {
                repo.Update(order);
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {

            try
            {
                repo.Save(order);
            }
            catch (DbUpdateException)
            {
                if (OrderExists(order.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = repo.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }

           repo.Delete(order);

            return Ok();
        }

        private bool OrderExists(int id)
        {
            return repo.Exist(id);
        }
    }
}
