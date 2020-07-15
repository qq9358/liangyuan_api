using System;
using System.Collections.Generic;
using System.Text;
using Egoal.Cryptography;
using Egoal.Domain.Entities.Auditing;

namespace Egoal.Staffs
{
    public class Staff : AuditedEntity
    {
        public string Uid { get; set; }
        public string Pwd { get; set; }
        public string Salt { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public int? DeptId { get; set; }
        public int? PostId { get; set; }
        public int? MerchantId { get; set; }
        public string CertNo { get; set; }
        public string EmployeeNo { get; set; }
        public int? TicketTypeGroupId { get; set; }
        public int? TicketTypeSearchGroupId { get; set; }
        public int? DiscountTypeGroupId { get; set; }
        public int? SalePointId { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public string PdaUid { get; set; }
        public string PdaPwd { get; set; }
        public bool? StatusFlag { get; set; }
        public string LeaveDate { get; set; }
        public string Memo { get; set; }
        public int? ParkId { get; set; }
        public Guid SyncCode { get; set; }
        public DateTime? SyncTime { get; set; }

        /// <summary>
        /// 已离职
        /// </summary>
        public bool HasLeft()
        {
            return StatusFlag == false;
        }

        public void EditPassword(string password)
        {
            var encryptPwd = SHAHelper.SHA512Encrypt(password, Salt);
            var encryptPwd1 = SHAHelper.SHA512Encrypt(encryptPwd, Id.ToString());

            Pwd = encryptPwd1;
        }
    }
}
