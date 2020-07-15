using System;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MustFillInAttribute : RequiredAttribute
    {
        public MustFillInAttribute()
        {
            ErrorMessage = "{0}不能为空";
        }
    }
}
