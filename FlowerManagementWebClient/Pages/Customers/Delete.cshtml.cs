using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using FlowerManagementWebClient.ViewModels;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;

namespace FlowerManagementWebClient.Pages.Customers
{
    public class DeleteModel : PageModel
    {
        private HttpClient client = null;
        private string CustomerApiUrl = "";
        public DeleteModel() { }

        [BindProperty]
        public CustomerViewModel Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            CustomerApiUrl = "https://localhost:44344/api/Customers/" + id;
            HttpResponseMessage response = await client.GetAsync(CustomerApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var customer = JsonSerializer.Deserialize<Customer>(strData, options);
            var customerViewmodel = new CustomerViewModel
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                Email = customer.Email,
                City = customer.City,
                Country = customer.Country,
                Password = customer.Password,
                Birthday = customer.Birthday
            };
            Customer = customerViewmodel;
            if (Customer == null)
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
            CustomerApiUrl = "https://localhost:44344/api/Customers/" + id;
            HttpResponseMessage response = await client.DeleteAsync(CustomerApiUrl);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
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
