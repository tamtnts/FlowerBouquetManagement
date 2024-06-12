using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FlowerManagementWebClient.Pages.FlowerBouquets
{
    public class DeleteModel : PageModel
    {
        private HttpClient client = null;
        private string FlowerBouquetApiUrl = "";

        public DeleteModel() { }

        [BindProperty]
        public FlowerBouquet FlowerBouquet { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            FlowerBouquetApiUrl = "https://localhost:44344/api/FlowerBouquets/" + id;
            HttpResponseMessage response = await client.GetAsync(FlowerBouquetApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            FlowerBouquet = JsonSerializer.Deserialize<FlowerBouquet>(strData, options);
            if (FlowerBouquet == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            FlowerBouquetApiUrl = "https://localhost:44344/api/FlowerBouquets/" + id;
            HttpResponseMessage response = await client.DeleteAsync(FlowerBouquetApiUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                return Page();
            }
        }
    }
}
