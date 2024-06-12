using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class FlowerBouquetDAO
    {

        public static IList<FlowerBouquet> GetFlowers()
        {
            var list = new List<FlowerBouquet>();
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    list = context.FlowerBouquets.Include(c => c.Category).ToList();
                    list = context.FlowerBouquets.Include(s => s.Supplier).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return list;
        }

        public static FlowerBouquet GetFlower(int id)
        {
            FlowerBouquet flowerBouquet = new FlowerBouquet();
            var list = new List<FlowerBouquet>();
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    list = context.FlowerBouquets.Include(c => c.Category)
                                                .Include(s => s.Supplier).ToList();
                    flowerBouquet = list.SingleOrDefault(f => f.FlowerBouquetId == id);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return flowerBouquet;

        }
        public static void Save(FlowerBouquet flowerBouquet)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    context.FlowerBouquets.Add(flowerBouquet);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Update(FlowerBouquet flowerBouquet)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    context.Entry<FlowerBouquet>(flowerBouquet).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void Delete(FlowerBouquet flowerBouquet)
        {
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    bool o = context.OrderDetails.Any(o => o.FlowerBouquetId == flowerBouquet.FlowerBouquetId);
                    var _flowerBouquet = context.FlowerBouquets.SingleOrDefault(f => f.FlowerBouquetId == flowerBouquet.FlowerBouquetId);
                    if (o)
                    {
                        
                        _flowerBouquet.FlowerBouquetStatus = 0;
                        context.SaveChanges();
                    }
                    else
                    {
                        context.FlowerBouquets.Remove(_flowerBouquet);
                        context.SaveChanges();

                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static IList<FlowerBouquet> SearchByName(string name)
        {
            var list = new List<FlowerBouquet>();
            try
            {
                using (var context = new FUFlowerBouquetManagementContext())
                {
                    if (name != null)
                    {
                        list = context.FlowerBouquets
                        .Include(f => f.Category)
                        .Include(f => f.Supplier)
                        .Where(f => f.FlowerBouquetName.Contains(name))
                        .Where(f => !f.FlowerBouquetStatus.Equals("Deleted"))
                        .ToList();
                    }
                    else
                    {
                        list = context.FlowerBouquets
                        .Include(f => f.Category)
                        .Include(f => f.Supplier)
                        .Where(f => !f.FlowerBouquetStatus.Equals("Deleted"))
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
                return context.FlowerBouquets.Any(f => f.FlowerBouquetId == id);
            }
        }
    }
}
