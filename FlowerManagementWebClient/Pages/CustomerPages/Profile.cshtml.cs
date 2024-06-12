using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlowerManagementWebClient.Pages.CustomerPages
{
    public class ProfileModel : PageModel
    {
        public ProfileModel() { }

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
