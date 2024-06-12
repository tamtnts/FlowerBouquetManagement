using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OrderDAO
    {
        public static IList<Order> GetOrders()
        {
            var list = new List<Order>();
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    list = context.Orders.Include(c => c.Customer).Where(c => !c.OrderStatus.Equals("Deleted")).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return list;
        }

        public static IList<Order> GetOrdersForReport(DateTime startDate, DateTime endDate)
        {
            var list = new List<Order>();
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    list = context.Orders.Include(c => c.Customer).Where(c => !c.OrderStatus.Equals("Deleted"))
                        .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate).OrderByDescending(o => o.Total).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return list;
        }

        public static Order GetOrder(int id)
        {
            Order order = new Order();
            var list = new List<Order>();
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    list = context.Orders.Include(o => o.Customer).ToList();
                    order = list.SingleOrDefault(o => o.OrderId == id);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return order;

        }

        public static IList<Order> GetOrderByCustomerId(int customerId)
        {
            var list = new List<Order>();
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    list = context.Orders.Include(c => c.Customer).Where(o => o.CustomerId == customerId).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return list;

        }
        public static void Save(Order order)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    context.Orders.Add(order);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }


        }
        public static void Update(Order order)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    context.Entry<Order>(order).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void Delete(Order order)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    var p1 = context.Orders.SingleOrDefault(c => c.OrderId == order.OrderId);
                    p1.OrderStatus = "Deleted";
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static bool Exist(int id)
        {
            using (var context = new FUFlowerBouquetManagementContext())
            {
                return context.Orders.Any(e => e.OrderId == id);
            }
        }
    }
}
    

