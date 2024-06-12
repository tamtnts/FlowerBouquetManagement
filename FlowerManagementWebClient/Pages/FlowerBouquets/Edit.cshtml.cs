using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using System.Net.Http;
using FlowerManagementWebClient.ViewModels;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace FlowerManagementWebClient.Pages.FlowerBouquets
{
    public class EditModel : PageModel
    {
        private HttpClient client = null;
        private string FlowerBouquetApiUrl = "";

        public EditModel() { }

        [BindProperty]
        public FlowerBouquetViewModel FlowerBouquet { get; set; }

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
            var flowerBouquet = JsonSerializer.Deserialize<FlowerBouquet>(strData, options);
            var flowerBouquetViewModel = new FlowerBouquetViewModel
            {
                FlowerBouquetId = flowerBouquet.FlowerBouquetId,
                FlowerBouquetName = flowerBouquet.FlowerBouquetName,
                CategoryId = flowerBouquet.CategoryId,
                Description = flowerBouquet.Description,
                UnitPrice = flowerBouquet.UnitPrice,
                UnitsInStock = flowerBouquet.UnitsInStock,
                FlowerBouquetStatus = flowerBouquet.FlowerBouquetStatus,
                SupplierId = flowerBouquet.SupplierId
            };
            FlowerBouquet = flowerBouquetViewModel;
            if (FlowerBouquet == null)
            {
                return NotFound();
            }
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            //ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                client = new HttpClient();
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                FlowerBouquetApiUrl = "https://localhost:44344/api/FlowerBouquets";
                string param = $"/{FlowerBouquet.FlowerBouquetId}";
                var flowerBouquet = new FlowerBouquet
                {
                    FlowerBouquetId = FlowerBouquet.FlowerBouquetId,
                    FlowerBouquetName = FlowerBouquet.FlowerBouquetName,
                    CategoryId = FlowerBouquet.CategoryId,
                    Description = FlowerBouquet.Description,
                    UnitPrice = FlowerBouquet.UnitPrice,
                    UnitsInStock = FlowerBouquet.UnitsInStock,
                    FlowerBouquetStatus = FlowerBouquet.FlowerBouquetStatus,
                    SupplierId = FlowerBouquet.SupplierId
                };
                var jsonObject = JsonSerializer.Serialize(flowerBouquet);
                HttpContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(FlowerBouquetApiUrl + param, content);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    return Page();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
