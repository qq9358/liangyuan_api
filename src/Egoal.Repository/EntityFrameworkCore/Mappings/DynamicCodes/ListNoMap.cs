using Egoal.DynamicCodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.DynamicCodes
{
    public class ListNoMap : IEntityTypeConfiguration<ListNo>
    {
        public void Configure(EntityTypeBuilder<ListNo> entity)
        {
            entity.HasKey(p => new { p.ListNoTypeID, p.ListDate });

            entity.Property(t => t.ListNoTypeID)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(t => t.ListDate)
                .IsRequired()
                .HasMaxLength(8);

            entity.Property(p => p.Id)
                .HasColumnName("ID")
                .IsConcurrencyToken();

            entity.ToTable("SM_ListNo");
        }
    }
}
