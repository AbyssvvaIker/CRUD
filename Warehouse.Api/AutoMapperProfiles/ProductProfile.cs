using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Api.DTOs.Product;
using Warehouse.Core.Entities;

namespace Warehouse.Web.AutoMapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap()
                .ForMember(p => p.Id, opt => opt.Ignore());
        }
    }
}
