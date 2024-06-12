using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CustomerDAO
    {
        public static Customer Login(string email, string password)
        {
            using (var context = new FUFlowerBouquetManagementContext())
            {
                return context.Customers.SingleOrDefault(c => c.Email == email && c.Password == password);
            }
        }

        public static IList<Customer> GetCustomers()
        {
            var list = new List<Customer>();
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    list = context.Customers.Include(c => c.Orders).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return list;
        }

        public static Customer GetCustomer(int id)
        {
            var list = new List<Customer>();
            Customer customer = new Customer();
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    list = context.Customers.Include(c => c.Orders).ToList();
                    customer = list.SingleOrDefault(c => c.CustomerId == id);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return customer;

        }
        public static void SaveCustomer(Customer customer)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    context.Customers.Add(customer);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Update(Customer customer)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    context.Entry<Customer>(customer).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Delete(Customer customer)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    var _customer = context.Customers.SingleOrDefault(c => c.CustomerId == customer.CustomerId);
                    context.Customers.Remove(_customer);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static IList<Customer> SearchByName(string name)
        {
            var list = new List<Customer>();
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    if (name != null)
                    {
                        list = context.Customers
                            .Include(c => c.Orders)
                            .Where(c => c.CustomerName.Contains(name))
                            .ToList();
                    }
                    else
                    {
                        list = context.Customers
                            .Include(c => c.Orders)
                            .ToList();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return list;
        }

        public static bool Exist(int id)
        {
            using (var context = new FUFlowerBouquetManagementContext())
            {
                return context.Customers.Any(c => c.CustomerId == id);
            }
        }
    }
}
