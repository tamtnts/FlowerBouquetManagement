using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlowerManagementWebClient.Pages.CustomerPages
{
    public class OrderHistoryDetailsModel : PageModel
    {
        public OrderHistoryDetailsModel() { }

        public IList<OrderDetail> OrderDetail { get; set; }

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
            OrderId = id;
            return Page();
        }
    }
}
