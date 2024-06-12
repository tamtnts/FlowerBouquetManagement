using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlowerManagementWebClient.Pages.Orders
{
    public class ReportModel : PageModel
    {
        public ReportModel() { }

        public IList<Order> Order { get; set; }

        [BindProperty]
        public string StartDate { get; set; }

        [BindProperty]
        public string EndDate { get; set; }

        public int Number { get; set; }
        public decimal? Total { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Number = 0; Total = 0;
            Order = new List<Order>();
            return Page();
        }

            public async Task<IActionResult> OnPostAsync()
        {
            HttpClient client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            string OrderApiUrl = "https://localhost:44344/api/Orders/Report";
            string param = $"?startDateStr={StartDate}&endDateStr={EndDate}";
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl + param);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Order = JsonSerializer.Deserialize<IList<Order>>(strData, options);
            Number = Order.Count;
            Total = 0;
            if (Order.Count > 0) {
                foreach (var item in Order)
                {
                    Total += item.Total;
                }
            }
            return Page();
        }
    }
}
