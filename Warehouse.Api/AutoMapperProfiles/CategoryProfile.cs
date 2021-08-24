using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Api.DTOs.Category;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;

namespace Warehouse.Web.AutoMapperProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap()
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<Guid, Category>()
                .ConvertUsing<GuidTocategoryConverter>();
            CreateMap<Category, Guid>()
                .ConvertUsing<CategoryToGuidConverter>();
        }
    }


    public class GuidTocategoryConverter : ITypeConverter<Guid, Category>
    {
        public readonly ICategoryLogic _categoryLogic;
        public GuidTocategoryConverter(ICategoryLogic categoryLogic)
        {
            _categoryLogic = categoryLogic;
        }

        public Category Convert(Guid source, Category destination, ResolutionContext context)
        {
            var result = _categoryLogic.GetByIdAsync(source).Result;
            if(result.Success == false)
            {
                return null;
            }
            return result.Value;
        }
    }

    public class CategoryToGuidConverter : ITypeConverter<Category, Guid>
    {
        public Guid Convert(Category source, Guid destination, ResolutionContext context)
        {
            return source.Id;
        }
    }

}
