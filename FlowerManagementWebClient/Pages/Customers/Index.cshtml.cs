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

namespace FlowerManagementWebClient.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private HttpClient client = null;
        private string CustomerApiUrl = "";

        public IndexModel() { }

        public IList<Customer> Customer { get;set; }

        [BindProperty]
        public string SearchString { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            CustomerApiUrl = "https://localhost:44344/api/Customers";
            HttpResponseMessage response = await client.GetAsync(CustomerApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Customer = JsonSerializer.Deserialize<IList<Customer>>(strData, options);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType); 
            CustomerApiUrl = "https://localhost:44344/api/Customers/Search";
            string param = $"?name={SearchString}";
            HttpResponseMessage response = await client.GetAsync(CustomerApiUrl + param);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Customer = JsonSerializer.Deserialize<IList<Customer>>(strData, options);
            return Page();
        }
    }
}
