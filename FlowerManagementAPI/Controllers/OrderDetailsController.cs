using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Repository.OrderDetailRepo;

namespace FlowerManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailRepo repo = new OrderDetailRepo();

                // GET: api/OrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IList<OrderDetail>>> GetOrderDetailByOrderId(int id)
        {
            IList<OrderDetail> list = repo.GetOrderDetailByOrderId(id);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        // PUT: api/OrderDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                repo.Update(orderDetail);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NoContent();
            }
            return Ok();
        }

        // POST: api/OrderDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderDetail>> PostOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                repo.Save(orderDetail);
                return Ok();
            }
            catch (DbUpdateException)
            {
                if (OrderDetailExists(orderDetail.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE: api/OrderDetails/5
         [HttpDelete]
         public async Task<IActionResult> DeleteOrderDetail(int orderId, int flowerBouquetId)
         {
            var orderDetail = repo.SearchOrderDetailByOrderIdAndByFlowerBouquetId(orderId,flowerBouquetId);
            if (orderDetail == null)
            {
                return NotFound();
            }
             repo.Delete(orderDetail);
             return Ok();
         }

        [HttpGet("GetOrderDetail")]
        public async Task<ActionResult<IList<OrderDetail>>> SearchOrderDetailByOrderIdAndByFlowerId(int orderId, int flowerBouquetId)
        {
            var orderDetail = repo.SearchOrderDetailByOrderIdAndByFlowerBouquetId(orderId, flowerBouquetId);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return Ok(orderDetail);
        }

        private bool OrderDetailExists(int id)
        {
            return repo.Exist(id);
        }
    }
}
