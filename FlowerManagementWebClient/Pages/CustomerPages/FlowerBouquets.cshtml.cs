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
    public class FlowerBouquetsModel : PageModel
    {
        private HttpClient client = null;
        private string FlowerBouquetApiUrl = "";

        public FlowerBouquetsModel() { }

        public IList<FlowerBouquet> FlowerBouquet { get; set; }

        [BindProperty]
        public string SearchString { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            FlowerBouquetApiUrl = "https://localhost:44344/api/FlowerBouquets";
            HttpResponseMessage response = await client.GetAsync(FlowerBouquetApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            FlowerBouquet = JsonSerializer.Deserialize<IList<FlowerBouquet>>(strData, options);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            FlowerBouquetApiUrl = "https://localhost:44344/api/FlowerBouquets/Search";
            string param = $"?name={SearchString}";
            HttpResponseMessage response = await client.GetAsync(FlowerBouquetApiUrl + param);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            FlowerBouquet = JsonSerializer.Deserialize<IList<FlowerBouquet>>(strData, options);
            return Page();
        }
    }
}
