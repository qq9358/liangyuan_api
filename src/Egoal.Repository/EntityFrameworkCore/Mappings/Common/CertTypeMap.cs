using Egoal.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Common
{
    public class CertTypeMap : IEntityTypeConfiguration<CertType>
    {
        public void Configure(EntityTypeBuilder<CertType> entity)
        {
            entity.ToTable("SM_CertType");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.SortCode)
                .HasMaxLength(50);
        }
    }
}
