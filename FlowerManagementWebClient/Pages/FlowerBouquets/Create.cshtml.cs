using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Models;
using FlowerManagementWebClient.ViewModels;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace FlowerManagementWebClient.Pages.FlowerBouquets
{
    public class CreateModel : PageModel
    {
        private HttpClient client = null;
  
        public CreateModel() { }

        public async Task<IActionResult> OnGet()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            string CategoryApiUrl = "https://localhost:44344/api/Categories";
            HttpResponseMessage responseCategory = await client.GetAsync(CategoryApiUrl);
            string strDataCategory = await responseCategory.Content.ReadAsStringAsync();
            Category = JsonSerializer.Deserialize<IList<Category>>(strDataCategory, options);
            string SupplierApiUrl = "https://localhost:44344/api/Suppliers";
            HttpResponseMessage responseSupplier = await client.GetAsync(SupplierApiUrl);
            string strDataSupplier = await responseSupplier.Content.ReadAsStringAsync();
            Supplier = JsonSerializer.Deserialize<IList<Supplier>>(strDataSupplier, options);
            ViewData["CategoryId"] = new SelectList(Category, "CategoryId", "CategoryName");
            ViewData["SupplierId"] = new SelectList(Supplier, "SupplierId", "SupplierName");
            return Page();
        }

        [BindProperty]
        public FlowerBouquetViewModel FlowerBouquet { get; set; }
        public IList<Category> Category { get; set; }
        public IList<Supplier> Supplier { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(Category, "CategoryId", "CategoryName");
                ViewData["SupplierId"] = new SelectList(Supplier, "SupplierId", "SupplierName");
                return Page();
            }
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            string FlowerBouquetApiUrl = "https://localhost:44344/api/FlowerBouquets";
            var flowerBouquet = new FlowerBouquet
            {
                FlowerBouquetId = FlowerBouquet.FlowerBouquetId,
                FlowerBouquetName = FlowerBouquet.FlowerBouquetName,
                Description = FlowerBouquet.Description,
                UnitPrice = FlowerBouquet.UnitPrice,
                UnitsInStock = FlowerBouquet.UnitsInStock,
                CategoryId = FlowerBouquet.CategoryId,
                SupplierId = FlowerBouquet.SupplierId,
                FlowerBouquetStatus = FlowerBouquet.FlowerBouquetStatus,
            };
            var jsonObject = JsonSerializer.Serialize(flowerBouquet);
            HttpContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(FlowerBouquetApiUrl, content);
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
