using Egoal.Staffs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Staffs
{
    public class StaffMap : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> entity)
        {
            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Address)
                .HasMaxLength(100);

            entity.Property(e => e.CertNo)
                .HasMaxLength(50);

            entity.Property(e => e.CID)
                .HasColumnName("CID");

            entity.Property(e => e.CTime)
                .HasColumnName("CTime")
                .HasColumnType("datetime");

            entity.Property(e => e.DeptId)
                .HasColumnName("DeptID");

            entity.Property(e => e.DiscountTypeGroupId)
                .HasColumnName("DiscountTypeGroupID");

            entity.Property(e => e.LeaveDate)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.Memo)
                .HasMaxLength(100);

            entity.Property(e => e.MerchantId)
                .HasColumnName("MerchantID");

            entity.Property(e => e.MID)
                .HasColumnName("MID");

            entity.Property(e => e.MTime)
                .HasColumnName("MTime")
                .HasColumnType("datetime");

            entity.Property(e => e.Name)
                .HasMaxLength(50);

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.PdaPwd)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.Property(e => e.PdaUid)
                .HasMaxLength(50);

            entity.Property(e => e.PostId)
                .HasColumnName("PostID");

            entity.Property(e => e.Pwd)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.Property(e => e.SalePointId)
                .HasColumnName("SalePointID");

            entity.Property(e => e.Salt)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Sex)
                .HasMaxLength(1);

            entity.Property(e => e.SyncTime)
                .HasColumnType("datetime");

            entity.Property(e => e.Tel).HasMaxLength(50);

            entity.Property(e => e.EmployeeNo)
                .HasMaxLength(20);

            entity.Property(e => e.TicketTypeGroupId)
                .HasColumnName("TicketTypeGroupID");

            entity.Property(e => e.TicketTypeSearchGroupId)
                .HasColumnName("TicketTypeSearchGroupID");

            entity.Property(e => e.Uid)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(e => e.Uid)
                .IsUnique();

            entity.ToTable("RM_Staff");
        }
    }
}
