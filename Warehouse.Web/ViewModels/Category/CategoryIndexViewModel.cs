using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Web.ViewModels
{
    public class CategoryIndexViewModel
    {
        public IList<CategoryIndexItemViewModel> Categories { get; set; }
    
        public CategoryIndexViewModel()
        {
            Categories = new List<CategoryIndexItemViewModel>();
        }
    }

    public class CategoryIndexItemViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
