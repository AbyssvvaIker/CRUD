using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.WebMvc.ViewModels
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

        public IndexItemViewModel() { }
        public IndexItemViewModel(Guid Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
    }
}
