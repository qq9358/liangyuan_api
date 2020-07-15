using System;

namespace Egoal.Application.Services.Dto
{
    [Serializable]
    public class NameValueDto : NameValueDto<string>
    {
        public NameValueDto()
        {

        }

        public NameValueDto(string name, string value)
            : base(name, value)
        {

        }

        public NameValueDto(NameValue nameValue)
            : this(nameValue.Name, nameValue.Value)
        {

        }
    }

    [Serializable]
    public class NameValueDto<T> : NameValue<T>
    {
        public NameValueDto()
        {

        }

        public NameValueDto(string name, T value)
            : base(name, value)
        {

        }

        public NameValueDto(NameValue<T> nameValue)
            : this(nameValue.Name, nameValue.Value)
        {

        }
    }
}
