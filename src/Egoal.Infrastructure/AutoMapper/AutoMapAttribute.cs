using Egoal.Extensions;
using Nelibur.ObjectMapper;
using System;

namespace Egoal.AutoMapper
{
    public class AutoMapAttribute : AutoMapAttributeBase
    {
        public AutoMapAttribute(params Type[] targetTypes)
            : base(targetTypes)
        {

        }

        public override void CreateMap(Type type)
        {
            if (TargetTypes.IsNullOrEmpty())
            {
                return;
            }

            foreach (var targetType in TargetTypes)
            {
                TinyMapper.Bind(type, targetType);
                TinyMapper.Bind(targetType, type);
            }
        }
    }
}
