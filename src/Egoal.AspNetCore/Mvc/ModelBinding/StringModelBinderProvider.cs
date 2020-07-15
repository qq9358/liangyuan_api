using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Egoal.Mvc.ModelBinding
{
    public class StringModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(string))
            {
                return new StringModelBinder(context.Metadata.ModelType, context.Services.GetService<ILoggerFactory>());
            }

            return null;
        }
    }
}
