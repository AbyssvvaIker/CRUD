using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Warehouse.Web.ExtensionMethods
{
    public static class ResultExtensions
    {
        public static void AddErrorToModelState(this Result result, ModelStateDictionary modelStateDictionary)
        {
            foreach(var item in result.Errors)
            {
                modelStateDictionary.AddModelError(item.PropertyName, item.Message);
            }
        }
    }
}
