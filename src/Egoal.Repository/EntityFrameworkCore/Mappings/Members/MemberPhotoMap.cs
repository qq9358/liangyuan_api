using Egoal.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Members
{
    public class MemberPhotoMap : IEntityTypeConfiguration<MemberPhoto>
    {
        public void Configure(EntityTypeBuilder<MemberPhoto> entity)
        {
            entity.HasKey(e => e.Id)
                    .ForSqlServerIsClustered(false);

            entity.HasIndex(e => e.MemberId);

            entity.Property(e => e.Id)
                .HasColumnName("ID")
                .ValueGeneratedNever();

            entity.Property(e => e.Ctime)
                .HasColumnName("CTime")
                .HasColumnType("datetime");

            entity.Property(e => e.MemberId)
                .HasColumnName("MemberID");

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.ParkName)
                .HasMaxLength(50);

            entity.ToTable("MM_MemberPhoto");

            entity.HasOne(e => e.Member)
                .WithMany(m => m.MemberPhotos)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
