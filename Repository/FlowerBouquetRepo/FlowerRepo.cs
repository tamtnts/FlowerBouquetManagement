using BusinessObject.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.FlowerBouquetRepo
{
    public class FlowerRepo : IFlowerRepo
    {
        public void Save(FlowerBouquet flowerBouquet) => FlowerBouquetDAO.Save(flowerBouquet);
        public void Delete(FlowerBouquet flowerBouquet) => FlowerBouquetDAO.Delete(flowerBouquet);
        public void Update(FlowerBouquet flowerBouquet) => FlowerBouquetDAO.Update(flowerBouquet);
        public IList<FlowerBouquet> GetFlowers() => FlowerBouquetDAO.GetFlowers();
        public FlowerBouquet GetFlower(int id) => FlowerBouquetDAO.GetFlower(id);
        public bool Exist(int id) => FlowerBouquetDAO.Exist(id);
        IList<FlowerBouquet> SearchByName(string name) => FlowerBouquetDAO.SearchByName(name);
    }
}
