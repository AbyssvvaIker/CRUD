using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;
using Warehouse.Web.ViewModels;
using Warehouse.Web.ViewModels.Category;

namespace Warehouse.Web.AutoMapperProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();
            CreateMap<Category, SelectItemViewModel>();
            CreateMap<Guid, Category>();
            CreateMap<Category, Guid>();
        }
    }

    public class ToSelectItemViewModelConverter : ITypeConverter<Category, SelectItemViewModel>
    {
        public SelectItemViewModel Convert(Category source, SelectItemViewModel destination, ResolutionContext context)
        {
            return new SelectItemViewModel
            {
                Display = source.Name,
                Value = source.Id.ToString()
            };
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
