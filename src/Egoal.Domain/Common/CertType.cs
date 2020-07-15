using Egoal.Domain.Entities;

namespace Egoal.Common
{
    public class CertType : Entity
    {
        public string Name { get; set; }
        public string SortCode { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>
        public string EnglishName { get; set; }
    }
}
