using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.WebMvc.ViewModels
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public CategoryViewModel() { }
        public CategoryViewModel(Guid Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }

    }

}
