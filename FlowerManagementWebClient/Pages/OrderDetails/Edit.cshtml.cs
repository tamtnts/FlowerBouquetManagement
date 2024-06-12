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
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System.Text;

namespace FlowerManagementWebClient.Pages.OrderDetails
{
    public class EditModel : PageModel
    {
        private HttpClient client = null;
        private string OrderDetailApiUrl = "";
        public EditModel() { }

        [BindProperty]
        public OrderDetailViewModel OrderDetail { get; set; }

        [ViewData]
        public int? OrderId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int? id2)
        {
            if (id == null)
            {
                return NotFound();
            }
            OrderId = id;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderDetailApiUrl = "https://localhost:44344/api/OrderDetails/GetOrderDetail";
            string param = $"?orderId={id}&flowerBouquetId={id2}";
            HttpResponseMessage response = await client.GetAsync(OrderDetailApiUrl+param);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var orderDetail = JsonSerializer.Deserialize<OrderDetail>(strData, options);
            var orderDetailViewModel = new OrderDetailViewModel
            {
                OrderId = orderDetail.OrderId,
                FlowerBouquetId = orderDetail.FlowerBouquetId,
                Quantity = orderDetail.Quantity,
                UnitPrice = orderDetail.UnitPrice,
                Discount = orderDetail.Discount
            };
            OrderDetail = orderDetailViewModel;
            if (OrderDetail == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            OrderId = OrderDetail.OrderId;
            if (!ModelState.IsValid)
            {
                return Page();
            }
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderDetailApiUrl = "https://localhost:44344/api/OrderDetails";
            var orderDetail = new OrderDetailViewModel
            {
                OrderId = OrderDetail.OrderId,
                FlowerBouquetId = OrderDetail.FlowerBouquetId,
                Quantity = OrderDetail.Quantity,
                Discount = OrderDetail.Discount,
                UnitPrice = OrderDetail.UnitPrice
            };
            var jsonObject = JsonSerializer.Serialize(orderDetail);
            HttpContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(OrderDetailApiUrl, content);
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
