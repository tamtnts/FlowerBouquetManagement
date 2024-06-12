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
    public class OrderHistoryModel : PageModel
    {
        public OrderHistoryModel() { }

        public IList<Order> Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            HttpClient client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            string OrderApiUrl = "https://localhost:44344/api/Orders/ByCustomer/" + id;
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Order = JsonSerializer.Deserialize<IList<Order>>(strData, options);
            return Page();
        }
    }
}
