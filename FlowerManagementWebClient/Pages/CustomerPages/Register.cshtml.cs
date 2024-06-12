using BusinessObject.Models;
using FlowerManagementWebClient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace FlowerManagementWebClient.Pages.CustomerPages
{
    public class RegisterModel : PageModel
    {
        public RegisterModel() { }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CustomerViewModel Customer { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            HttpClient client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            string CustomerApiUrl = "https://localhost:44344/api/Customers";
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
            HttpResponseMessage response = await client.PostAsync(CustomerApiUrl, content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                return Page();
            }
        }
    }
}
