using BusinessObject;
using BusinessObject.Models;
using FlowerManagementWebClient.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlowerManagementWebClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public LoginViewModel LoginModel { get; set; }

        [ViewData]
        public string Message { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnPost()
        {
            HttpClient client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            string CustomerApiUrl = "https://localhost:44344/api/Customers/Login";
            string param = $"?email={LoginModel.Email}&password={LoginModel.Password}";
            HttpContent content = new StringContent(param, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(CustomerApiUrl + param, content);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string strData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var result = JsonSerializer.Deserialize<Customer>(strData, options);
                HttpContext.Session.SetInt32("id", result.CustomerId);
                return RedirectToPage("/CustomerPages/FlowerBouquets");
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return RedirectToPage("/Customers/Index");
            }
            else
            {
                Message = "Incorrect email or password";
            }
            return Page();
        }
    }
}
