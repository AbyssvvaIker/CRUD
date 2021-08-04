using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Core.Entities
{
    public class Category : BaseEntity
    {

        
        public string Name { get; set; }

    }
}
