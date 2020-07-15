using System;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MaximumLengthAttribute : MaxLengthAttribute
    {
        public MaximumLengthAttribute(int length)
            : base(length)
        {
            ErrorMessage = "{0}长度不能大于{1}";
        }
    }
}
