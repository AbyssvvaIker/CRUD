using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Core.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }

        public BaseEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
