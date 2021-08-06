using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Web.ViewModels
{
    public class ProductIndexViewModel
    {
        public IList<ProductIndexItemViewModel> Products { get; set; }

        public ProductIndexViewModel()
        {
            Products = new List<ProductIndexItemViewModel>();
        }
    }

    public class ProductIndexItemViewModel
    {
        public Guid Id { get; set; }
        //public string Category { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
