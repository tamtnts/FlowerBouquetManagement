using BusinessObject.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.FlowerBouquetRepo
{
    public interface IFlowerRepo
    {
         void Save(FlowerBouquet flowerBouquet);
         void Delete(FlowerBouquet flowerBouquet);
         void Update(FlowerBouquet flowerBouquet);
         IList<FlowerBouquet> GetFlowers();
         FlowerBouquet GetFlower(int id);
         bool Exist(int id);
        public IList<FlowerBouquet> SearchByName(string name) => FlowerBouquetDAO.SearchByName(name);
    }
}
