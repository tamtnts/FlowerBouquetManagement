using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace FlowerManagementWebClient.Pages.OrderDetails
{
    public class IndexModel : PageModel
    {
        public IndexModel() { }

        public IList<OrderDetail> OrderDetail { get;set; }

        [ViewData]
        public int? OrderId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            HttpClient client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            string OrderDetailApiUrl = "https://localhost:44344/api/OrderDetails/" + id;
            HttpResponseMessage response = await client.GetAsync(OrderDetailApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            OrderDetail = JsonSerializer.Deserialize<IList<OrderDetail>>(strData, options);
            decimal total = 0;
            foreach (var item in OrderDetail)
            {
                total += (item.UnitPrice * item.Quantity) * (decimal)((100 - item.Discount) / 100);
            }
            OrderId = id;
            client = new HttpClient();
            string OrderApiUrl = "https://localhost:44344/api/Orders/" + OrderId;
            HttpResponseMessage responseOrder = await client.GetAsync(OrderApiUrl);
            strData = await responseOrder.Content.ReadAsStringAsync();
            Order order = JsonSerializer.Deserialize<Order>(strData, options);
            client = new HttpClient();
            OrderApiUrl = "https://localhost:44344/api/Orders";
            string param = $"/{OrderId}";
            var newOrder = new Order
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus,
                ShippedDate = order.ShippedDate,
                CustomerId = order.CustomerId,
                Total = total
            };
            var jsonObject = JsonSerializer.Serialize(newOrder);
            HttpContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            responseOrder = await client.PutAsync(OrderApiUrl + param, content);
            return Page();
        }
    }
}
