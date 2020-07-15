using System;
using System.Collections.Generic;

namespace Egoal.Application.Services.Dto
{
    [Serializable]
    public class ComboboxItemDto : ComboboxItemDto<string>
    {
        public ComboboxItemDto()
        {
        }

        public ComboboxItemDto(string value, string displayText)
        {
            Value = value;
            DisplayText = displayText;
        }
    }

    [Serializable]
    public class ComboboxItemDto<T>
    {
        public T Value { get; set; }
        public string DisplayText { get; set; }
        public bool Disabled { get; set; }

        public ComboboxItemDto()
        {
        }

        public ComboboxItemDto(T value, string displayText)
        {
            Value = value;
            DisplayText = displayText;
        }
    }

    public class ComboboxItemDtoComparer<T> : IEqualityComparer<ComboboxItemDto<T>>
    {
        public bool Equals(ComboboxItemDto<T> x, ComboboxItemDto<T> y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            return x.Value.Equals(y.Value);
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.
        public int GetHashCode(ComboboxItemDto<T> obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 0;
            }

            return obj.Value.GetHashCode();
        }
    }
}
