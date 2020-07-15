using Egoal.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using Egoal.Extensions;
using Egoal.UI;
using Egoal.Cryptography;

namespace Egoal.Members
{
    public class Member : AuditedEntity<Guid>
    {
        public const int OpenIdLength = 28;

        public Member()
        {
            Id = Guid.NewGuid();
            MemberPhotos = new List<MemberPhoto>();
        }

        public RegType? RegTypeId { get; set; }
        public string Uid { get; set; }
        public string Pwd { get; set; }
        public string Salt { get; set; }
        public string Code { get; set; }
        public string OldCode { get; set; }
        public string Name { get; set; }
        public string PetName { get; set; }
        public string Zjf { get; set; }
        public string Sex { get; set; }
        public string Birth { get; set; }
        public string Mobile { get; set; }
        public int? CertTypeId { get; set; }
        public string CertTypeName { get; set; }
        public string CertNo { get; set; }
        public string CertDate { get; set; }
        public int? MemberTypeId { get; set; }
        public string MemberTypeName { get; set; }
        public int? MemberLevelId { get; set; }
        public string MemberLevelName { get; set; }
        public int? MaxTicketSaleNumPerDay { get; set; }
        public MemberStatus? MemberStatusId { get; set; }
        public string MemberStatusName { get; set; }
        public string CompanyName { get; set; }
        public string BusinessLicense { get; set; }
        public string TravelAgencyLicense { get; set; }
        public string LegalPerson { get; set; }
        public string LegalPersonTel { get; set; }
        public string InvoiceTitle { get; set; }
        public string Taxid { get; set; }
        public string BankAccountNumber { get; set; }
        public string JiDiaoName { get; set; }
        public string JiDiaoTel { get; set; }
        public string Tel { get; set; }
        public string Nation { get; set; }
        public string Education { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Qq { get; set; }
        public string Msn { get; set; }
        public string WeChatNo { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string Linkman { get; set; }
        public int? InnRoomNum { get; set; }
        public int? InnBedNum { get; set; }
        public string WebSite { get; set; }
        public int? KeYuanTypeId { get; set; }
        public string KeYuanTypeName { get; set; }
        public int? AreaId { get; set; }
        public string AreaName { get; set; }
        public DateTime? Etime { get; set; }
        public string Memo { get; set; }
        public int? BookFailTimes { get; set; }
        public bool? BlacklistFlag { get; set; }
        public DateTime? BlacklistETime { get; set; }
        public string CardId { get; set; }
        public string CardNo { get; set; }
        public string CardStime { get; set; }
        public string CardEtime { get; set; }
        public int? TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public int? TicketStatusId { get; set; }
        public string TicketStatusName { get; set; }
        public bool? CardValidFlag { get; set; }
        public string CardValidFlagName { get; set; }
        public string HeadImgUrl { get; set; }
        public int? StaffId { get; set; }
        public int? CCID { get; set; }
        public DateTime? CCTime { get; set; }
        public int? ParkId { get; set; }
        public string ParkName { get; set; }
        //public string ReportStatus { get; set; }
        //public string ReportStatusName { get; set; }
        //public DateTime? AuditTime { get; set; }
        //public Guid? AuthId { get; set; }
        public string SourceType { get; set; }

        /// <summary>
        /// 是否关注微信公众账号
        /// </summary>
        public bool? IsWeChatSubscribed { get; set; }

        public virtual ICollection<MemberPhoto> MemberPhotos { get; set; }

        public virtual UserWechat UserWechat { get; set; }

        public bool IsBlacklisted()
        {
            if (BlacklistFlag == true)
            {
                if (!BlacklistETime.HasValue) return true;

                return BlacklistETime.Value > DateTime.Now;
            }

            return false;
        }

        public void AddPhoto(string photo)
        {
            if (photo.IsNullOrEmpty())
            {
                return;
            }

            var photoString = photo.Split(',')[1];
            var memberPhoto = new MemberPhoto();
            memberPhoto.Photo = Convert.FromBase64String(photoString);
            memberPhoto.Ctime = DateTime.Now;
            MemberPhotos.Add(memberPhoto);
        }

        public void BindStaff(int staffId)
        {
            StaffId = staffId;
        }

        public void Audit(bool agree, string reason)
        {
            if (MemberStatusId != MemberStatus.审核中)
            {
                throw new UserFriendlyException($"状态为：{MemberStatusId?.ToString()}，无法审核");
            }

            MemberStatusId = agree ? MemberStatus.正常 : MemberStatus.已禁用;
            MemberStatusName = MemberStatusId?.ToString();
            Memo = reason;
        }

        public void ChangePassword(string originalPassword, string newPassword)
        {
            var pwd = SHAHelper.SHA512Encrypt(originalPassword, Salt);
            if (pwd != Pwd)
            {
                throw new UserFriendlyException("原密码不正确");
            }

            Pwd = SHAHelper.SHA512Encrypt(newPassword, Salt);
        }
    }
}
