using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Web.ViewModels.Product;

namespace Warehouse.Web.AutoMapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap()
                .ForMember(p => p.Id, opt => opt.Ignore());
            CreateMap<Product, IndexItemViewModel>();
        }
    }
}
