using BusinessObject.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.CustomerRepo
{
    public class CustomerRepo : ICustomerRepo
    {
        public Customer Login(string email, string password) => CustomerDAO.Login(email, password);
        public void Save(Customer customer) => CustomerDAO.SaveCustomer(customer);
        public void Delete(Customer customer) => CustomerDAO.Delete(customer);
        public void Update(Customer customer) => CustomerDAO.Update(customer);
        public IList<Customer> GetCustomers() => CustomerDAO.GetCustomers();
        public Customer GetCustomer(int id) => CustomerDAO.GetCustomer(id);
        public bool Exist(int id) => CustomerDAO.Exist(id);
        public IList<Customer> SearchByName(string name) => CustomerDAO.SearchByName(name);
    }
}
