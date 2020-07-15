using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Egoal.Mvc.ModelBinding
{
    public class StringModelBinder : IModelBinder
    {
        private readonly SimpleTypeModelBinder _simpleTypeModelBinder;

        public StringModelBinder(Type type, ILoggerFactory loggerFactory)
        {
            _simpleTypeModelBinder = new SimpleTypeModelBinder(type, loggerFactory);
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            await _simpleTypeModelBinder.BindModelAsync(bindingContext);

            if (!bindingContext.Result.IsModelSet)
            {
                return;
            }

            var s = (string)bindingContext.Result.Model;
            bindingContext.Result = ModelBindingResult.Success(s?.Trim());
        }
    }
}
