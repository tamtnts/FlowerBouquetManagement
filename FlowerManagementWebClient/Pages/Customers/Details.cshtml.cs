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

namespace FlowerManagementWebClient.Pages.Customers
{
    public class DetailsModel : PageModel
    {
        public DetailsModel() { }

        public Customer Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            HttpClient client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            string CustomerApiUrl = "https://localhost:44344/api/Customers/" + id;
            HttpResponseMessage response = await client.GetAsync(CustomerApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Customer = JsonSerializer.Deserialize<Customer>(strData, options);
            if (Customer == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
