using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class CartItem
    {
        public FlowerBouquet FlowerBouquet { get; set; }
        public int Quantity { get; set; }
    }
}
