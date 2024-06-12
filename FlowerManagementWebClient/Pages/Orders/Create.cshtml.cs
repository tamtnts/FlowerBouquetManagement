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
using FlowerManagementWebClient.ViewModels;
using System.Net.Http.Headers;
using System.Text;

namespace FlowerManagementWebClient.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private HttpClient client = null;
        private string OrderApiUrl = "";

        public CreateModel() {}

        public async Task<IActionResult> OnGet()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            client = new HttpClient();
            string CustomerApiUrl = "https://localhost:44344/api/Customers";
            HttpResponseMessage responseCustomer = await client.GetAsync(CustomerApiUrl);
            string strDataCustomer = await responseCustomer.Content.ReadAsStringAsync();
            var customers = JsonSerializer.Deserialize<IList<Customer>>(strDataCustomer, options);
            ViewData["CustomerId"] = new SelectList(customers, "CustomerId", "CustomerName");
            return Page();
        }

        [BindProperty]
        public OrderViewModel Order { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            client = new HttpClient();
            string CustomerApiUrl = "https://localhost:44344/api/Customers";
            HttpResponseMessage responseCustomer = await client.GetAsync(CustomerApiUrl);
            string strDataCustomer = await responseCustomer.Content.ReadAsStringAsync();
            var customers = JsonSerializer.Deserialize<IList<Customer>>(strDataCustomer, options);
            ViewData["CustomerId"] = new SelectList(customers, "CustomerId", "CustomerName");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderApiUrl = "https://localhost:44344/api/Orders";
            var order = new Order
            {
                OrderId = Order.OrderId,
                OrderDate = Order.OrderDate,
                OrderStatus = Order.OrderStatus,
                ShippedDate = Order.ShippedDate,
                CustomerId = Order.CustomerId,
                Total = Order.Total
            };
            var jsonObject = JsonSerializer.Serialize(order);
            HttpContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(OrderApiUrl, content);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
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
