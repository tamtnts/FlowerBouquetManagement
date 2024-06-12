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

namespace FlowerManagementWebClient.Pages.Orders
{
    public class IndexModel : PageModel
    {
        public IndexModel() { }

        public IList<Order> Order { get;set; }

        public async Task<IActionResult> OnGetAsync()
        {
            HttpClient client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            string OrderApiUrl = "https://localhost:44344/api/Orders";
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
