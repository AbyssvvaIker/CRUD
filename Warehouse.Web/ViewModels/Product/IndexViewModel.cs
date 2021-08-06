using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Web.ViewModels.Product
{
    public class IndexViewModel
    {
        public IList<IndexItemViewModel> Products { get; set; }

        public IndexViewModel()
        {
            Products = new List<IndexItemViewModel>();
        }
    }

    public class IndexItemViewModel
    {
        public Guid Id { get; set; }
        //public string Category { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
