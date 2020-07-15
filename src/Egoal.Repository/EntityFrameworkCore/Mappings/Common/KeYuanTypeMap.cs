using Egoal.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Common
{
    public class KeYuanTypeMap : IEntityTypeConfiguration<KeYuanType>
    {
        public void Configure(EntityTypeBuilder<KeYuanType> entity)
        {
            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.SortCode)
                .HasMaxLength(50);

            entity.ToTable("OM_KeYuanType");
        }
    }
}
