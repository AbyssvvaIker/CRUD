﻿using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;
using Warehouse.Core.Interfaces.Repositories;

namespace Warehouse.Infrastructure.DataAccess.Repositories
{
    class ProductRepository : Repository<Product>, IProductRepository 
    {
        public ProductRepository(DataContext db) : base(db)
        {

        }
    }
}
