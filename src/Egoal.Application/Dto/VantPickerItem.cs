using System.Collections.Generic;

namespace Egoal.Dto
{
    public class VantPickerItem<T>
    {
        public VantPickerItem()
        {
            Children = new List<VantPickerItem<T>>();
        }

        public string Text { get; set; }
        public T Value { get; set; }
        public bool Disabled { get; set; }
        public List<VantPickerItem<T>> Children { get; set; }
    }
}
