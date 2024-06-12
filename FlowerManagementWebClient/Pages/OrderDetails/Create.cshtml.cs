using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Models;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;
using FlowerManagementWebClient.ViewModels;
using System.Text;

namespace FlowerManagementWebClient.Pages.OrderDetails
{
    public class CreateModel : PageModel
    {
        private HttpClient client = null;
        private string OrderDetailApiUrl = "";

        public CreateModel() { }

        public async Task<IActionResult> OnGet(int? id)
        {
            OrderId = id;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            string FlowerBouquetApiUrl = "https://localhost:44344/api/FlowerBouquets";
            HttpResponseMessage responseFlowerBouquet = await client.GetAsync(FlowerBouquetApiUrl);
            string strDataFlowerBouquet = await responseFlowerBouquet.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            FlowerBouquets = JsonSerializer.Deserialize<IList<FlowerBouquet>>(strDataFlowerBouquet, options);
            ViewData["FlowerBouquetId"] = new SelectList(FlowerBouquets, "FlowerBouquetId", "FlowerBouquetName");
            return Page();
        }

        [BindProperty]
        public OrderDetailViewModel OrderDetail { get; set; }

        [ViewData]
        public int? OrderId { get; set; }

        [ViewData]
        public string Message { get; set; }

        IList<FlowerBouquet> FlowerBouquets { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            OrderId = id;
            if (!ModelState.IsValid)
            {
                ViewData["FlowerBouquetId"] = new SelectList(FlowerBouquets, "FlowerBouquetId", "FlowerBouquetName");
                return Page();
            }
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderDetailApiUrl = "https://localhost:44344/api/OrderDetails";
            var orderDetail = new OrderDetailViewModel
            {
                OrderId = (int)id,
                FlowerBouquetId = OrderDetail.FlowerBouquetId,
                Quantity = OrderDetail.Quantity,
                Discount = OrderDetail.Discount,
                UnitPrice = OrderDetail.UnitPrice
            };
            var jsonObject = JsonSerializer.Serialize(orderDetail);
            HttpContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(OrderDetailApiUrl, content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToPage("./Index", new {id = OrderId});
            }
            else
            {
                client = new HttpClient();
                string FlowerBouquetApiUrl = "https://localhost:44344/api/FlowerBouquets";
                HttpResponseMessage responseFlowerBouquet = await client.GetAsync(FlowerBouquetApiUrl);
                string strDataFlowerBouquet = await responseFlowerBouquet.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                FlowerBouquets = JsonSerializer.Deserialize<IList<FlowerBouquet>>(strDataFlowerBouquet, options);
                Message = "This flower bouquet is already exist!";
                ViewData["FlowerBouquetId"] = new SelectList(FlowerBouquets, "FlowerBouquetId", "FlowerBouquetName");
                return Page();
            }
        }
    }
}
