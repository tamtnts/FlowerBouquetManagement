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

namespace FlowerManagementWebClient.Pages.OrderDetails
{
    public class DeleteModel : PageModel
    {
        private HttpClient client = null;
        private string OrderDetailApiUrl = "";

        public DeleteModel() { }

        [BindProperty]
        public OrderDetail OrderDetail { get; set; }

        [ViewData]
        public int? OrderId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int? id2)
        {
            if (id == null || id2 == null)
            {
                return NotFound();
            }
            OrderId = id;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderDetailApiUrl = "https://localhost:44344/api/OrderDetails/GetOrderDetail";
            string param = $"?orderId={id}&flowerBouquetId={id2}";
            HttpResponseMessage response = await client.GetAsync(OrderDetailApiUrl + param);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            OrderDetail = JsonSerializer.Deserialize<OrderDetail>(strData, options);
            if (OrderDetail == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderDetailApiUrl = "https://localhost:44344/api/OrderDetails";
            string param = $"?orderId={OrderDetail.OrderId}&flowerBouquetId={OrderDetail.FlowerBouquetId}";
            HttpResponseMessage response = await client.DeleteAsync(OrderDetailApiUrl + param);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToPage("./Index", new { id = OrderDetail.OrderId });
            }
            else
            {
                return Page();
            }
        }
    }
}
