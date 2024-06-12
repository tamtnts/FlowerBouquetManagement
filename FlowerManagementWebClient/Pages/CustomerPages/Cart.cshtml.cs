using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BusinessObject;
using FlowerManagementWebClient.Helpers;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using Microsoft.AspNetCore.Http;
using FlowerManagementWebClient.ViewModels;

namespace FlowerManagementWebClient.Pages.CustomerPages
{
    public class CartModel : PageModel
    {
        private HttpClient client = null;

        public CartModel() { }

        public IList<CartItem> cart { get; set; }
        public decimal Total { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if(cart == null)
            {
                cart = new List<CartItem>();
            }
            Total = cart.Sum(i => i.FlowerBouquet.UnitPrice * i.Quantity);
            return Page();
        }

        public async Task<IActionResult> OnGetBuyNow(int id)
        {
            var flowerBouquet = new FlowerBouquet();
            cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            string FlowerBouquetApiUrl = "https://localhost:44344/api/FlowerBouquets/" + id;
            HttpResponseMessage response = await client.GetAsync(FlowerBouquetApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var flowerBouquetObject = JsonSerializer.Deserialize<FlowerBouquet>(strData, options);
            if (flowerBouquetObject == null)
            {
                return NotFound();
            }
            if (cart == null)
            {
                cart = new List<CartItem>();
                cart.Add(new CartItem
                {
                    FlowerBouquet = flowerBouquetObject,
                    Quantity = 1
                });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                int index = Exists(cart, id);
                if (index == -1)
                {
                    cart.Add(new CartItem
                    {
                        FlowerBouquet = flowerBouquetObject,
                        Quantity = 1
                    });
                }
                else
                {
                    cart[index].Quantity++;
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            Total = cart.Sum(i => i.FlowerBouquet.UnitPrice * i.Quantity);
            return Page();
        }

        public IActionResult OnGetDelete(int id)
        {
            cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            int index = Exists(cart, id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            Total = cart.Sum(i => i.FlowerBouquet.UnitPrice * i.Quantity);
            return Page();
        }

        public IActionResult OnPostUpdate(int[] quantities)
        {
            cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            for (var i = 0; i < cart.Count; i++)
            {
                cart[i].Quantity = quantities[i];
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            Total = cart.Sum(i => i.FlowerBouquet.UnitPrice * i.Quantity);
            return Page();
        }

        public async Task<IActionResult> OnPostCheckout()
        {
            cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                cart = new List<CartItem>();   
            }
            if(cart.Count == 0)
            {
                return Page();
            }
            Total = cart.Sum(i => i.FlowerBouquet.UnitPrice * i.Quantity);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            string OrderApiUrl = "https://localhost:44344/api/Orders";
            var customerId = HttpContext.Session.GetInt32("id");
            Random rand = new Random();
            int orderId = rand.Next(1, 5000);
            var order = new Order
            {
                OrderId = orderId,  
                OrderDate = DateTime.Now,
                OrderStatus = "CheckedOut",
                ShippedDate = DateTime.Now.AddDays(1),
                CustomerId = customerId,
                Total = Total
            };
            var jsonObject = JsonSerializer.Serialize(order);
            HttpContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(OrderApiUrl, content);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                for (var i = 0; i < cart.Count(); i++)
                {
                    client = new HttpClient();
                    string OrderDetailApiUrl = "https://localhost:44344/api/OrderDetails";
                    var orderDetail = new OrderDetailViewModel
                    {
                        OrderId = orderId,
                        FlowerBouquetId = cart[i].FlowerBouquet.FlowerBouquetId,
                        Quantity = cart[i].Quantity,
                        Discount = 0,
                        UnitPrice = cart[i].Quantity * cart[i].FlowerBouquet.UnitPrice
                    };
                    jsonObject = JsonSerializer.Serialize(orderDetail);
                    content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                    response = await client.PostAsync(OrderDetailApiUrl, content);
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return Page();
                    }
                }
                for(var i = 0; i < cart.Count(); i++)
                {
                    cart.RemoveAt(i);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
            }
            else
            {
                return Page();
            }
            return RedirectToPage("./OrderHistory", new {id = customerId});
        }

        private int Exists(IList<CartItem> cart, int id)
        {
            for (var i = 0; i < cart.Count; i++)
            {
                if (cart[i].FlowerBouquet.FlowerBouquetId == id)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
