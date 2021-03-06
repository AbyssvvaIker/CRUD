using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
