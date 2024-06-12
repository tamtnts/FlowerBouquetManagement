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
using Microsoft.Extensions.Options;

namespace FlowerManagementWebClient.Pages.Orders
{
    public class EditModel : PageModel
    {
        private HttpClient client = null;
        private string OrderApiUrl = "";

        public EditModel() { }

        [BindProperty]
        public OrderViewModel Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderApiUrl = "https://localhost:44344/api/Orders/" + id;
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var order = JsonSerializer.Deserialize<Order>(strData, options);
            var orderViewModel = new OrderViewModel
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus,
                ShippedDate = order.ShippedDate,
                CustomerId = order.CustomerId,
                Total = order.Total
            };
            Order = orderViewModel;
            if (Order == null)
            {
                return NotFound();
            }
            string CustomerApiUrl = "https://localhost:44344/api/Customers";
            HttpResponseMessage responseCustomer = await client.GetAsync(CustomerApiUrl);
            string strDataCustomer = await responseCustomer.Content.ReadAsStringAsync();
            var customers = JsonSerializer.Deserialize<IList<Customer>>(strDataCustomer, options);
            ViewData["CustomerId"] = new SelectList(customers, "CustomerId", "CustomerName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                client = new HttpClient();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                string CustomerApiUrl = "https://localhost:44344/api/Customers";
                HttpResponseMessage responseCustomer = await client.GetAsync(CustomerApiUrl);
                string strDataCustomer = await responseCustomer.Content.ReadAsStringAsync();
                var customers = JsonSerializer.Deserialize<IList<Customer>>(strDataCustomer, options);
                ViewData["CustomerId"] = new SelectList(customers, "CustomerId", "CustomerName");
                return Page();
            }
            try
            {
                client = new HttpClient();
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                OrderApiUrl = "https://localhost:44344/api/Orders";
                string param = $"/{Order.OrderId}";
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
                HttpResponseMessage response = await client.PutAsync(OrderApiUrl + param, content);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return RedirectToPage("/Orders/Index");
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
