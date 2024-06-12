using BusinessObject.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.CustomerRepo
{
    public interface ICustomerRepo
    {
        Customer Login(string email, string password);
        void Save(Customer customer);
        void Delete(Customer customer);
        void Update(Customer customer);
        IList<Customer> GetCustomers();
        Customer GetCustomer(int id);
        bool Exist(int id);
        IList<Customer> SearchByName(string name);
    }
}
