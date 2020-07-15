using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Egoal.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class IdentityCardNoAttribute : ValidationAttribute
    {
        public IdentityCardNoAttribute()
        {
            ErrorMessage = "{0}格式不正确";
        }

        public override bool IsValid(object value)
        {
            string stringValue = Convert.ToString(value, CultureInfo.CurrentCulture);

            if (string.IsNullOrEmpty(stringValue))
            {
                return true;
            }

            if (stringValue.Length != 15 && stringValue.Length != 18)
            {
                return false;
            }

            string pattern = stringValue.Length == 15 ?
                @"^[1-9]\d{5}\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{2}$" :
                @"^[1-9]\d{5}(19|20)\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(stringValue))
            {
                return false;
            }

            if (stringValue.Length == 18)
            {
                var weightingFactor = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
                int total = 0;
                for (int i = 0; i < weightingFactor.Length; i++)
                {
                    total += weightingFactor[i] * (Convert.ToInt32(stringValue[i]) - 48);
                }
                string code = "10X98765432";
                if (code[total % 11] != stringValue.ToUpper()[stringValue.Length - 1])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
