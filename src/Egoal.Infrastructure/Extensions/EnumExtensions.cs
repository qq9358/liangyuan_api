using Egoal.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Egoal.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 枚举转下拉列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<ComboboxItemDto<int>> ToComboboxItems(this Type type)
        {
            List<ComboboxItemDto<int>> items = new List<ComboboxItemDto<int>>();
            foreach (FieldInfo field in type.GetFields())
            {
                if (field.IsSpecialName) continue;
                ComboboxItemDto<int> item = new ComboboxItemDto<int>();
                var descriptions = field.GetCustomAttributes(typeof(DescriptionAttribute)) as DescriptionAttribute[];
                if (descriptions.Length > 0)
                {
                    item.DisplayText = descriptions[0].Description;
                }
                else
                {
                    item.DisplayText = field.Name;
                }
                item.Value = field.GetRawConstantValue().To<int>();
                items.Add(item);
            }
            return items;
        }

        /// <summary>
        /// 获取描述信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            var descriptions = field.GetCustomAttributes(typeof(DescriptionAttribute)) as DescriptionAttribute[];
            if (descriptions.Length > 0)
            {
                return descriptions[0].Description;
            }
            return field.Name;
        }

        /// <summary>
        /// 获取最大长度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetMaxLength(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(MaxLengthAttribute)) as MaxLengthAttribute[];
            if (attributes.Length > 0)
            {
                return attributes[0].Length;
            }
            return 0;
        }
    }
}
