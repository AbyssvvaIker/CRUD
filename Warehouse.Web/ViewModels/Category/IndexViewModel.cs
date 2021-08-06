using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Web.ViewModels.Category
{
    public class IndexViewModel
    {
        public IList<IndexItemViewModel> Categories { get; set; }
    
        public IndexViewModel()
        {
            Categories = new List<IndexItemViewModel>();
        }
    }

    public class IndexItemViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
