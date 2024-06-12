using BusinessObject.Models;
using FlowerManagementWebClient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System;

namespace FlowerManagementWebClient.Pages.CustomerPages
{
    public class EditProfileModel : PageModel
    {
        private HttpClient client = null;
        private string CustomerApiUrl = "";

        public EditProfileModel() { }

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
                CustomerApiUrl = "https://localhost:44344/api/Customers";
                string param = $"/{Customer.CustomerId}";
                var customer = new Customer
                {
                    CustomerId = Customer.CustomerId,
                    CustomerName = Customer.CustomerName,
                    Email = Customer.Email,
                    City = Customer.City,
                    Country = Customer.Country,
                    Password = Customer.Password,
                    Birthday = Customer.Birthday
                };
                var jsonObject = JsonSerializer.Serialize(customer);
                HttpContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(CustomerApiUrl + param, content);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return RedirectToPage("./Profile", new {id = Customer.CustomerId});
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
