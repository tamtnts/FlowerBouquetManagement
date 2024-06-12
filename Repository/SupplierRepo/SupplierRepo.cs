using BusinessObject.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SupplierRepo
{
    public class SupplierRepo : ISupplierRepo
    {
        public IList<Supplier> GetSuppliers() => SupplierDAO.GetSuppliers();
    }
}
