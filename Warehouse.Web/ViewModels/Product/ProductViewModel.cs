using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Web.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public Guid Category { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public IEnumerable<SelectItemViewModel> AvailableCategories { get; set; }
    }

}
