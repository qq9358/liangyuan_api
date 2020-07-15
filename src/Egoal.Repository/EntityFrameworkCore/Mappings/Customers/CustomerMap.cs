using Egoal.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Customers
{
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> entity)
        {
            entity.HasKey(e => e.Id)
                    .ForSqlServerIsClustered(false);

            entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

            entity.Property(e => e.Address)
                .HasMaxLength(100);

            entity.Property(e => e.AreaId)
                .HasColumnName("AreaID");

            entity.Property(e => e.AreaName)
                .HasMaxLength(50);

            entity.Property(e => e.BusinessLicense)
                .HasMaxLength(50)
                .IsUnicode(false);

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

            entity.Property(e => e.CustomerLevelId)
                .HasColumnName("CustomerLevelID");

            entity.Property(e => e.CustomerLevelName)
                .HasMaxLength(50);

            entity.Property(e => e.CustomerStatusId)
                .HasColumnName("CustomerStatusID");

            entity.Property(e => e.CustomerStatusName)
                .HasMaxLength(50);

            entity.Property(e => e.CustomerTypeId)
                .HasColumnName("CustomerTypeID");

            entity.Property(e => e.CustomerTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.Email)
                .HasMaxLength(50);

            entity.Property(e => e.Fax)
                .HasMaxLength(50);

            entity.Property(e => e.LegalPerson)
                .HasMaxLength(50);

            entity.Property(e => e.Linkman)
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

            entity.Property(e => e.Salt)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.SortCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Tel)
                .HasMaxLength(50);

            entity.Property(e => e.Uid)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.WebSite)
                .HasMaxLength(50);

            entity.Property(e => e.ZipCode)
                .HasMaxLength(6)
                .IsUnicode(false);

            entity.Property(e => e.Zjf)
                .HasMaxLength(50);

            entity.ToTable("CM_Customer");

            entity.HasIndex(e => e.Uid)
                .IsUnique()
                .ForSqlServerIsClustered();
        }
    }
}
