using Egoal.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Members
{
    public class MemberMap : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> entity)
        {
            entity.HasKey(e => e.Id)
                .ForSqlServerIsClustered(false);

            entity.Property(p => p.Id)
                .HasColumnName("ID")
                .ValueGeneratedNever();

            entity.Property(e => e.Address)
                .HasMaxLength(100);

            entity.Property(e => e.AreaId)
                .HasColumnName("AreaID");

            entity.Property(e => e.AreaName)
                .HasMaxLength(50);

            //entity.Property(e => e.AuditTime)
            //    .HasColumnType("datetime");

            //entity.Property(e => e.AuthId)
            //    .HasColumnName("AuthID");

            entity.Property(e => e.BankAccountNumber)
                .HasMaxLength(100);

            entity.Property(e => e.Birth)
                .HasMaxLength(10);

            entity.Property(e => e.BusinessLicense)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CardEtime)
                .HasColumnName("CardETime")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.CardId)
                .HasColumnName("CardID")
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CardNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CardStime)
                .HasColumnName("CardSTime")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.CardValidFlagName)
                .HasMaxLength(50);

            entity.Property(e => e.CCID)
                .HasColumnName("CCID");

            entity.Property(e => e.CCTime)
                .HasColumnName("CCTime")
                .HasColumnType("datetime");

            entity.Property(e => e.CertDate)
                .HasMaxLength(10);

            entity.Property(e => e.CertNo)
                .HasMaxLength(50);

            entity.Property(e => e.CertTypeId)
                .HasColumnName("CertTypeID");

            entity.Property(e => e.CertTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.CID)
                .HasColumnName("CID");

            entity.Property(e => e.Code)
                .HasMaxLength(50);

            entity.Property(e => e.CompanyName)
                .HasMaxLength(50);

            entity.Property(e => e.CTime)
                .HasColumnName("CTime")
                .HasColumnType("datetime");

            entity.Property(e => e.Email).HasMaxLength(50);

            entity.Property(e => e.Etime)
                .HasColumnName("ETime")
                .HasColumnType("datetime");

            entity.Property(e => e.Fax)
                .HasMaxLength(50);

            entity.Property(e => e.HeadImgUrl)
                .HasMaxLength(500);

            entity.Property(e => e.InvoiceTitle)
                .HasMaxLength(100);

            entity.Property(e => e.JiDiaoName)
                .HasMaxLength(50);

            entity.Property(e => e.JiDiaoTel)
                .HasMaxLength(50);

            entity.Property(e => e.KeYuanTypeId)
                .HasColumnName("KeYuanTypeID");

            entity.Property(e => e.KeYuanTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.LegalPerson)
                .HasMaxLength(50);

            entity.Property(e => e.LegalPersonTel)
                .HasMaxLength(50);

            entity.Property(e => e.Linkman)
                .HasMaxLength(50);

            entity.Property(e => e.MemberLevelId)
                .HasColumnName("MemberLevelID");

            entity.Property(e => e.MemberLevelName)
                .HasMaxLength(50);

            entity.Property(e => e.MemberStatusId)
                .HasColumnName("MemberStatusID");

            entity.Property(e => e.MemberStatusName)
                .HasMaxLength(50);

            entity.Property(e => e.MemberTypeId)
                .HasColumnName("MemberTypeID");

            entity.Property(e => e.MemberTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.Memo)
                .HasMaxLength(100);

            entity.Property(e => e.MID)
                .HasColumnName("MID");

            entity.Property(e => e.Mobile)
                .HasMaxLength(50);

            entity.Property(e => e.Msn)
                .HasColumnName("MSN")
                .HasMaxLength(50);

            entity.Property(e => e.MTime)
                .HasColumnName("MTime")
                .HasColumnType("datetime");

            entity.Property(e => e.Name)
                .HasMaxLength(50);

            entity.Property(e => e.OldCode)
                .HasMaxLength(50);

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.ParkName)
                .HasMaxLength(50);

            entity.Property(e => e.PetName)
                .HasMaxLength(50);

            entity.Property(e => e.Pwd)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.Property(e => e.Qq)
                .HasColumnName("QQ")
                .HasMaxLength(50);

            entity.Property(e => e.RegTypeId)
                .HasColumnName("RegTypeID");

            //entity.Property(e => e.ReportStatus)
            //    .HasMaxLength(10);

            //entity.Property(e => e.ReportStatusName)
            //    .HasMaxLength(20);

            entity.Property(e => e.Salt)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Sex)
                .HasMaxLength(1);

            entity.Property(e => e.SourceType)
                .HasMaxLength(10);

            entity.Property(e => e.Taxid)
                .HasColumnName("TAXID")
                .HasMaxLength(50);

            entity.Property(e => e.Tel)
                .HasMaxLength(50);

            entity.Property(e => e.Nation)
                .HasMaxLength(20);

            entity.Property(e => e.Education)
                .HasMaxLength(50);

            entity.Property(e => e.TicketStatusId)
                .HasColumnName("TicketStatusID");

            entity.Property(e => e.TicketStatusName)
                .HasMaxLength(50);

            entity.Property(e => e.TicketTypeId)
                .HasColumnName("TicketTypeID");

            entity.Property(e => e.TicketTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.TravelAgencyLicense)
                .HasMaxLength(50);

            entity.Property(e => e.Uid)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.WeChatNo)
                .HasMaxLength(50);

            entity.Property(e => e.WebSite)
                .HasMaxLength(50);

            entity.Property(e => e.ZipCode)
                .HasMaxLength(6)
                .IsUnicode(false);

            entity.Property(e => e.Zjf).HasMaxLength(50);

            entity.ToTable("MM_Member");

            entity.HasIndex(e => e.CertNo);

            entity.HasIndex(e => e.Mobile);

            entity.HasIndex(e => e.Uid)
                .IsUnique()
                .ForSqlServerIsClustered();
        }
    }
}
