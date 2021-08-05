using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Core.Entities
{
    class Product : BaseEntity
    {
        public string Category { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
