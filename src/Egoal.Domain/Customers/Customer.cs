using Egoal.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace Egoal.Customers
{
    public class Customer : AuditedEntity<Guid>
    {
        public Customer()
        {
            Id = Guid.NewGuid();
            CustomerPhotos = new List<CustomerPhoto>();
        }

        public string Uid { get; set; }
        public string Pwd { get; set; }
        public string Salt { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string PetName { get; set; }
        public string Zjf { get; set; }
        public string Mobile { get; set; }
        public int? CertTypeId { get; set; }
        public string CertTypeName { get; set; }
        public string CertNo { get; set; }
        public string CertDate { get; set; }
        public int? CustomerTypeId { get; set; }
        public string CustomerTypeName { get; set; }
        public CustomerLevel? CustomerLevelId { get; set; }
        public string CustomerLevelName { get; set; }
        public CustomerStatus? CustomerStatusId { get; set; }
        public string CustomerStatusName { get; set; }
        public bool? GzFlag { get; set; }
        public decimal? GzMaxMoney { get; set; }
        public string CompanyName { get; set; }
        public string BusinessLicense { get; set; }
        public string LegalPerson { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Qq { get; set; }
        public string Msn { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string Linkman { get; set; }
        public string WebSite { get; set; }
        public int? AreaId { get; set; }
        public string AreaName { get; set; }
        public string Memo { get; set; }
        public byte[] Logo { get; set; }
        public bool? IsWeixinDistributor { get; set; }
        public int? ParkId { get; set; }
        public string ParkName { get; set; }
        public string SortCode { get; set; }

        public ICollection<CustomerPhoto> CustomerPhotos { get; set; }
    }
}
