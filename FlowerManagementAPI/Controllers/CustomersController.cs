using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Repository;
using Repository.CustomerRepo;
using Microsoft.Data.SqlClient;
using BusinessObject;
using Microsoft.Extensions.Options;

namespace FlowerManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepo repository = new CustomerRepo();
        private AdminAccount adminAccount;

        public CustomersController(IOptions<AdminAccount> appSettings)
        {
            adminAccount = appSettings.Value;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAccount(string email, string password)
        {
            try
            {
                string adminUsername = adminAccount.Email;
                string adminPassword = adminAccount.Password;
                if (email.Equals(adminUsername) && password.Equals(adminPassword))
                {
                    return NoContent();
                }
                Customer customer = repository.Login(email,password);
                if(customer != null)
                {
                    return Ok(customer);
                }
                else
                {
                    return NotFound();
                }    
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IList<Customer>>> GetCustomers()
        {
            IList<Customer> customers = repository.GetCustomers();
            return Ok(customers);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = repository.GetCustomer(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            var p1 = repository.GetCustomer(id);
            if (p1 == null)
                return NotFound();
            repository.Update(customer);
            return Ok();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            try
            {
                repository.Save(customer);
                return Ok();
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.CustomerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = repository.GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }
            repository.Delete(customer);
            return Ok();
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IList<Customer>>> SearchByName(string name)
        {
            var items = repository.SearchByName(name);
            return Ok(items);
        }

        private bool CustomerExists(int id)
        {
            return repository.Exist(id);
        }
    }
}

