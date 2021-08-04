using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Core.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        
        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [StringLength(32, MinimumLength = 2)]
        public string Name { get; set; }

    }
}
