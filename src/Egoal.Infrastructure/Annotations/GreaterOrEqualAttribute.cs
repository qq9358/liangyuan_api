using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Egoal.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class GreaterOrEqualAttribute : CompareAttribute
    {
        public GreaterOrEqualAttribute(string otherProperty)
            : base(otherProperty)
        {
            ErrorMessage = "{0}不能小于{1}";
        }

        public new string OtherPropertyDisplayName { get; set; }

        public override string FormatErrorMessage(string name) =>
            string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, OtherPropertyDisplayName ?? OtherProperty);

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetRuntimeProperty(OtherProperty);
            if (otherPropertyInfo == null)
            {
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, "{0}属性不存在", OtherProperty));
            }
            if (otherPropertyInfo.GetIndexParameters().Any())
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "{0}.{1}属性不存在", validationContext.ObjectType.FullName, OtherProperty));
            }

            object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

            if (!(otherPropertyValue is IComparable))
            {
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, "{0}属性不是可比较类型", OtherProperty));
            }
            if (!(value is IComparable))
            {
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, "{0}属性不是可比较类型", validationContext.MemberName));
            }

            if ((value as IComparable).CompareTo(otherPropertyValue as IComparable) < 0)
            {
                if (OtherPropertyDisplayName == null)
                {
                    OtherPropertyDisplayName = GetDisplayNameForProperty(otherPropertyInfo);
                }

                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return null;
        }

        private string GetDisplayNameForProperty(PropertyInfo property)
        {
            var attributes = CustomAttributeExtensions.GetCustomAttributes(property, true);
            var display = attributes.OfType<DisplayAttribute>().FirstOrDefault();
            if (display != null)
            {
                return display.GetName();
            }

            return OtherProperty;
        }
    }
}
